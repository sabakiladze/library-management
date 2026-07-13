using Application.Implementations;
using Application.Implimentations;
using Application.Interfaces;
using Application.Validations;
using Domain.Models;
using Infrastructure.Repositories;
using LibraryManagementSystem.DataAccess.Interfaces;
using LibraryManagementSystem.DataAccess.Repositories;
using LibraryManagementSystem.Domain.Models;
using LibraryManagementSystem.Services.AuthService;


namespace management_ui_library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserSession userSession=new UserSession();

            FileRepository<User> userFileRepo = new("UserStorage.txt");
            FileRepository<Book> bookFileRepo = new("BookStorage.txt");
            FileRepository<LogInLog> logFileRepo = new("LogginInfo.txt");
            FileRepository<BorrowRecord> recordFileRepo = new("BorrowRequestInfo.txt");
            FileRepository<BorrowRequest> requestFileRepo = new("BorrowRequestInfo.txt");


            Validation validation = new Validation();

            UserRepository userRepo = new(userFileRepo);
            BookRepository bookRepo = new(bookFileRepo);
            BorrowRecordRepository recordRepo = new(recordFileRepo);
            BorrowRequestRepository requestRepo = new(requestFileRepo);



            AuthService authService = new(userRepo, userSession, logFileRepo, userFileRepo);
            BookService bookService = new(bookRepo, userSession,validation);

            BorrowService borrowSerice = new(validation, userSession, requestRepo);
            // აქ უნდა დავამატო borrow and request ის ობიექტები

        }
    }
}