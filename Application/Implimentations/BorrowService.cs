using Application.Interfaces;
using Application.Validations;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using LibraryManagementSystem.DataAccess.Interfaces;
using LibraryManagementSystem.Domain.Models;
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



        public void ApproveRequest(int requestId)
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


            Book book =
                _bookRepository.GetBookContainingCopy(copy.Id);


            _bookRepository.Update(book);



            request.Approve();

            _borrowRequestRepo.Update(request);



            BorrowRecord record = new(
                request.UserId,
                request.BookId,
                request.BookCopyId
            );


            _borrowRecordRepo.Add(record);
        }




        public List<BorrowRecord>? GetMyRecords()
        {
            return _borrowRecordRepo
                .GetByUserId(_userSession.CurrentUser.Id);
        }



        public List<BorrowRequest>? GetMyRequests()
        {
            return _borrowRequestRepo
                .GetRequestsByUserId(_userSession.CurrentUser.Id)
                ?? throw new InvalidUserIdOrUserHasNotHaveAnyRequestsYetException();
        }




        public List<BorrowRequest>? GetPendingRequests()
        {
            _validations.EnsureAdmin(_userSession);

            return _borrowRequestRepo.GetPendingRequests();
        }





        public void RejectRequest(int requestId)
        {
            _validations.EnsureAdmin(_userSession);


            BorrowRequest request =
                _borrowRequestRepo.GetRequestById(requestId)
                ?? throw new RequestByThisIdDoNotExists();


            if (request.Status != BookBorrowStatus.Pending)
                throw new InvalidOperationException("Request already processed");


            request.Reject();


            _borrowRequestRepo.Update(request);
        }





        public void RequestBook(int bookId)
        {
            User user =
                _userRepository.GetUserById(_userSession.CurrentUser.Id)
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



            _borrowRequestRepo.AddRequest(request);
        }





        public void ReturnBook(int recordId)
        {
            BorrowRecord record =
                _borrowRecordRepo.GetById(recordId)
                ?? throw new Exception("Record not found");



            if (record.UserId != _userSession.CurrentUser.Id)
                throw new UnauthorizedAccessException();



            if (record.IsReturned)
                throw new InvalidOperationException("Book already returned");



            BookCopy copy =
                _bookRepository.GetBookCopyByGuid(record.BookCopyId)
                ?? throw new BookNotFoundException();



            decimal fee = record.CalculateFee();



            copy.Return();



            Book book =
                _bookRepository.GetBookContainingCopy(copy.Id);


            _bookRepository.Update(book);




            record.ReturnBook();


            _borrowRecordRepo.Update(record);




            if (fee > 0)
            {
                User user =
                    _userRepository.GetUserById(record.UserId)
                    ?? throw new UserNotFound();


                user.AddFee(fee);


                _userRepository.Update(user);
            }
        }
    }
}