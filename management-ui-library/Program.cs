using Application.Implementations;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Repositories;
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

            FileRepository<User> userFileRepo = new FileRepository<User>("UserStorage.txt");
            FileRepository<Book> bookFileRepo = new FileRepository<Book>("BookStorage.txt");
            FileRepository<LogInLog> logFileRepo = new FileRepository<LogInLog>("LogginInfo.txt");

            UserRepository userRepo = new UserRepository(userFileRepo);
            BookRepository bookRepo = new BookRepository(bookFileRepo);

            AuthService authService = new AuthService(userRepo, userSession, logFileRepo, userFileRepo);
            BookService bookService = new BookService(bookRepo, userSession);

        }
    }
}