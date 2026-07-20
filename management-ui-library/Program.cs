using Application.Implementations;
using Application.Implimentations;
using Application.Validations;
using Domain.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using LibraryManagementSystem.DataAccess.Repositories;
using LibraryManagementSystem.Domain.Models;
using management_ui_library.Menus;
using Microsoft.Extensions.Configuration;



namespace management_ui_library
{
    public class Program
    {
        static void Main(string[] args)
        {


            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

           
            EmailSettings settings = new()
            {
                Email = configuration["EmailSettings:Email"],
                AppPassword = configuration["EmailSettings:AppPassword"],
                SmtpServer = configuration["EmailSettings:SmtpServer"],
                Port = int.Parse(configuration["EmailSettings:Port"])
            };


            EmailService emailService = new(settings);

            UserSession userSession=new();

            FileRepository<User> userFileRepo = new("UsersStorage.txt");
            FileRepository<Book> bookFileRepo = new("BookStorage.txt");
            FileRepository<LogInLog> logFileRepo = new("LogginInfo.txt");
            FileRepository<BorrowRecord> recordFileRepo = new("BorrowRecordInfo.txt");
            FileRepository<BorrowRequest> requestFileRepo = new("BorrowRequestInfo.txt");


            Validation validation = new();

            UserRepository userRepo = new(userFileRepo);
            BookRepository bookRepo = new(bookFileRepo);
            BorrowRecordRepository recordRepo = new(recordFileRepo);
            BorrowRequestRepository requestRepo = new(requestFileRepo);


            AuthService authService = new(userRepo, userSession, logFileRepo, emailService);
            BookService bookService = new(bookRepo, userSession,validation);

            BorrowService borrowSerice = new(validation, userSession, requestRepo, recordRepo, bookRepo, userRepo);
            UserService userService = new(userSession, userRepo, validation);

            UserMenu userMenu = new(bookService, borrowSerice, authService, userSession);
            AdminMenu adminMenu = new(bookService, borrowSerice, userService, authService, userSession);
            Menu menu = new(authService, userSession, userMenu, adminMenu);

            menu.Run();
        }
    }
}