using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Validations;
using Domain.Exceptions;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Enums.BookBorrowRequestStatus;
using static LibraryManagementSystem.Domain.Enums.BookStatus;

namespace Application.Implimentations
{
    public class BorrowService : IBorrowService
    {
        private readonly Validation _validations;
        private readonly UserSession _userSession;
        private readonly IBorrowRequestRepository _borrowRequestRepo;
        private readonly IBorrowReqordRepository _borrowRecordRepo;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public BorrowService(
            Validation validation,
            UserSession userSession,
            IBorrowRequestRepository borrowRequestRepo,
            IBorrowReqordRepository borrowRecordRepo,
            IBookRepository bookRepository,
            IUserRepository userRepository)
        {
            _validations = validation;
            _userSession = userSession;
            _borrowRequestRepo = borrowRequestRepo;
            _borrowRecordRepo = borrowRecordRepo;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task ApproveRequestAsync(int requestId)
        {
            _validations.EnsureAdmin(_userSession);

            BorrowRequest request =
                _borrowRequestRepo.GetRequestById(requestId)
                ?? throw new RequestByThisIdDoNotExists();

            if (request.Status != BookBorrowStatus.Pending)
                throw new InvalidOperationException("Request already processed");

            BookCopy copy =
                _bookRepository.GetBookCopyByGuid(request.BookCopyId)
                ?? throw new BookNotFoundException();

            copy.Borrow();

            Book book = _bookRepository.GetBookContainingCopy(copy.Id);

            await _bookRepository.UpdateAsync(book);

            request.Approve();

            await _borrowRequestRepo.UpdateAsync(request);

            BorrowRecord record = new(
                request.UserId,
                request.BookId,
                request.BookCopyId
            );

            await _borrowRecordRepo.AddAsync(record);
        }

        public List<BorrowRecord>? GetMyRecords()
        {
            _validations.EnsureLoggedIn(_userSession);

            return _borrowRecordRepo.GetByUserId(_userSession.CurrentUser!.Id);
        }

        public List<BorrowRequest>? GetMyRequests()
        {
            _validations.EnsureLoggedIn(_userSession);

            return _borrowRequestRepo
                .GetRequestsByUserId(_userSession.CurrentUser!.Id)
                ?? throw new InvalidUserIdOrUserHasNotHaveAnyRequestsYetException();
        }

        public List<BorrowRequest>? GetPendingRequests()
        {
            _validations.EnsureAdmin(_userSession);

            return _borrowRequestRepo.GetPendingRequests();
        }

        public async Task RejectRequestAsync(int requestId)
        {
            _validations.EnsureAdmin(_userSession);

            BorrowRequest request =
                _borrowRequestRepo.GetRequestById(requestId)
                ?? throw new RequestByThisIdDoNotExists();

            if (request.Status != BookBorrowStatus.Pending)
                throw new InvalidOperationException("Request already processed");

            request.Reject();

            await _borrowRequestRepo.UpdateAsync(request);
        }

        public async Task CancelRequestAsync(int requestId)
        {
            _validations.EnsureLoggedIn(_userSession);

            BorrowRequest request =
                _borrowRequestRepo.GetRequestById(requestId)
                ?? throw new RequestByThisIdDoNotExists();

            User currentUser = _userSession.CurrentUser!;
            bool isOwnRequest = request.UserId == currentUser.Id;
            bool isAdmin = currentUser.Role == LibraryManagementSystem.Domain.Enums.UserRole.Role.Admin;

            if (!isOwnRequest && !isAdmin)
                throw new UnauthorizedAccessException("You can only cancel your own requests.");

            if (request.Status != BookBorrowStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be cancelled.");

            request.Cancel();

            await _borrowRequestRepo.UpdateAsync(request);
        }

        public async Task RequestBookAsync(int bookId)
        {
            _validations.EnsureLoggedIn(_userSession);

            User user =
                _userRepository.GetUserById(_userSession.CurrentUser!.Id)
                ?? throw new UserNotFound();

            if (user.HasDebt())
                throw new UserMustPayFineFirst();

            Book book =
                _bookRepository.GetBookById(bookId)
                ?? throw new BookNotFoundException();

            BookCopy copy =
                book.Copies.FirstOrDefault(x =>
                    x.Status == Book_Status.Available)
                ?? throw new BookOutOfRangeException();

            BorrowRequest request = new(
                user.Id,
                book.Id,
                copy.Id
            );

            await _borrowRequestRepo.AddRequestAsync(request);
        }

        public async Task ReturnBookAsync(int recordId)
        {
            _validations.EnsureLoggedIn(_userSession);

            BorrowRecord record =
                _borrowRecordRepo.GetById(recordId)
                ?? throw new Exception("Record not found");

            if (record.UserId != _userSession.CurrentUser!.Id)
                throw new UnauthorizedAccessException();

            if (record.IsReturned)
                throw new InvalidOperationException("Book already returned");

            BookCopy copy =
                _bookRepository.GetBookCopyByGuid(record.BookCopyId)
                ?? throw new BookNotFoundException();

            decimal fee = record.CalculateFee();
            record.ChargeFee(fee);

            copy.Return();

            Book book = _bookRepository.GetBookContainingCopy(copy.Id);

            await _bookRepository.UpdateAsync(book);

            record.ReturnBook();

            await _borrowRecordRepo.UpdateAsync(record);

            if (fee > 0)
            {
                User user =
                    _userRepository.GetUserById(record.UserId)
                    ?? throw new UserNotFound();

                user.AddFee(fee);

                await _userRepository.UpdateAsync(user);
            }
        }
    }
}
