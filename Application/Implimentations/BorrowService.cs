using Application.Interfaces;
using Application.Validations;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using LibraryManagementSystem.DataAccess.Interfaces;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.BookBorrowRequestStatus;
using static LibraryManagementSystem.Domain.Enums.BookStatus;

namespace Application.Implimentations
{
    public class BorrowService : IBorrowService
    {
        private readonly Validation _validations;
        private readonly UserSession  _userSession;
        private readonly IBorrowRequestRepository _borrowRequestRepo;
        private readonly IBorrowReqordRepository _borrowReqordRepo;
        private readonly IBookRepository _bookRepository;

        public BorrowService(
            Validation validation, 
            UserSession userSession, 
            IBorrowRequestRepository borrowRequestRepo,
            IBorrowReqordRepository borrowReqordRepo,
            IBookRepository bookRepository)
        {
            _validations = validation;
            _userSession = userSession;
            _borrowRequestRepo = borrowRequestRepo;
            _borrowReqordRepo = borrowReqordRepo;
            _bookRepository = bookRepository; 
        }
        public void ApproveRequest(int requestId)
        {
            _validations.EnsureAdmin(_userSession);

            

            BorrowRequest ? request =
                _borrowRequestRepo.GetRequestById(requestId);

            if (request.Status != BookBorrowStatus.Pending)
                throw new InvalidOperationException("Request already processed");

            BookCopy ? copy =
                _bookRepository.GetBookCopyByGuid(request.BookCopyId)
                ?? throw new BookNotFoundException();


            copy.Borrow();


            Book book =// იმ წიგნს პოულობს რომლის ეგზემპლარიც არის ეს აი
                _bookRepository.GetBookContainingCopy(copy.Id);


            _bookRepository.Update(book);


            request.Approve();

            _borrowRequestRepo.Update(request); // რექვესთს იმიტომ ვანახლებ რომ მოთხოვნის სტატუსი შევცვალო


            BorrowRecord record = new(
                request.UserId,
                request.BookId,
                request.BookCopyId
            );


            _borrowReqordRepo.Add(record);
        }

        public List<BorrowRecord> ? GetMyRecords()
        {
            return  _borrowReqordRepo.GetByUserId(_userSession.CurrentUser.Id);

        }

        public List<BorrowRequest> ? GetMyRequests() => _borrowRequestRepo.GetRequestsByUserId(_userSession.CurrentUser.Id) ?? 
            throw new InvalidUserIdOrUserHasNotHaveAnyRequestsYetException();

        public List<BorrowRequest>? GetPendingRequests()
        {
          return _borrowRequestRepo.GetPendingRequests();
        }

        public void RejectRequest(int requestId)
        {
            _validations.EnsureAdmin(_userSession);
            BorrowRequest? request = _borrowRequestRepo.GetRequestById(requestId) ?? throw new RequestByThisIdDoNotExists();
            if (request.Status != BookBorrowStatus.Pending)
                throw new InvalidOperationException("Request already processed");
            request.Reject();
            _borrowRequestRepo.Update(request);
        }

        public void RequestBook(int bookId)
        {
            Book book = _bookRepository.GetBookById(bookId)
                ?? throw new BookNotFoundException();


            BookCopy copy = book.Copies
                .FirstOrDefault(x => x.Status == Book_Status.Available)
                ?? throw new BookOutOfRangeException();


            BorrowRequest request = new BorrowRequest(
                _userSession.CurrentUser.Id,
                book.Id,
                copy.Id
            );


            _borrowRequestRepo.AddRequest(request);
        }

        public void ReturnBook(int recordId)
        {
            BorrowRecord record =
                _borrowReqordRepo.GetById(recordId);


            if (record.UserId != _userSession.CurrentUser.Id)
                throw new UnauthorizedAccessException();


            if (record.IsReturned)
                throw new InvalidOperationException("Book already returned");


            BookCopy copy =
                _bookRepository.GetBookCopyByGuid(record.BookCopyId)
                ?? throw new BookNotFoundException();

            copy.Return();


            Book book =
                _bookRepository.GetBookContainingCopy(copy.Id);


            _bookRepository.Update(book);


            record.ReturnBook();

            _borrowReqordRepo.Update(record);

        }
    }
}
