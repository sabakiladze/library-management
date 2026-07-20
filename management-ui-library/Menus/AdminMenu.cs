using Application.Interfaces.Services;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using management_ui_library.Utils;
using System;
using System.Collections.Generic;

namespace management_ui_library.Menus
{
    public class AdminMenu
    {
        private readonly IBookService _bookService;
        private readonly IBorrowService _borrowService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly UserSession _userSession;

        public AdminMenu(
            IBookService bookService,
            IBorrowService borrowService,
            IUserService userService,
            IAuthService authService,
            UserSession userSession)
        {
            _bookService = bookService;
            _borrowService = borrowService;
            _userService = userService;
            _authService = authService;
            _userSession = userSession;
        }

        public void Run()
        {
            while (_userSession.IsLoggedIn)
            {
                User ? current = _userSession.CurrentUser;

                Console.WriteLine($"\n===== Admin menu ({current.UserName}) =====");
                Console.WriteLine("1. Full book catalog");
                Console.WriteLine("2. Add a book");
                Console.WriteLine("3. Add a copy of a book");
                Console.WriteLine("4. Delete a book");
                Console.WriteLine("5. Delete a copy");
                Console.WriteLine("6. Pending requests");
                Console.WriteLine("7. Approve a request");
                Console.WriteLine("8. Reject a request");
                Console.WriteLine("9. Promote a user to Admin");
                Console.WriteLine("10. Stats (books/copies)");
                Console.WriteLine("0. Log out");

                string choice = ConsoleHelper.ReadLineOrEmpty("Choose: ");

                switch (choice)
                {
                    case "1": ShowAllBooks(); break;
                    case "2": AddBook(); break;
                    case "3": AddCopy(); break;
                    case "4": DeleteBook(); break;
                    case "5": DeleteCopy(); break;
                    case "6": ShowPendingRequests(); break;
                    case "7": ApproveRequest(); break;
                    case "8": RejectRequest(); break;
                    case "9": PromoteToAdmin(); break;
                    case "10": ShowStats(); break;
                    case "0": _authService.LogOut(); break;
                    default: ConsoleHelper.PrintError("Invalid menu option."); break;
                }
            }
        }

        private void ShowAllBooks()
        {
            List<Book> books = _bookService.GetAllBooks();
            if (books.Count == 0)
            {
                Console.WriteLine("There are no books.");
                return;
            }

            foreach (Book b in books)
            {
                Console.WriteLine($"#{b.Id} | {b.Title} — {b.Author.FirstName} {b.Author.LastName} ({b.PublicationYear}) | copies: {b.Copies.Count}");
                foreach (BookCopy c in b.Copies)
                    Console.WriteLine($"    copy {c.Id} | status: {c.Status}");
            }
        }

        private void AddBook()
        {
            string title = ConsoleHelper.ReadNonEmpty("Title: ");
            string first = ConsoleHelper.ReadNonEmpty("Author's first name: ");
            string last = ConsoleHelper.ReadNonEmpty("Author's last name: ");
            int year = ConsoleHelper.ReadInt("Publication year: ");

            ConsoleHelper.TryRun(() =>
            {
                Author author = new() 
                { FirstName = first, LastName = last };
                Book book = new(title, author, year);
                _bookService.AddBook(book);
                ConsoleHelper.PrintSuccess($"Book added (ID: {book.Id}).");
            });
        }

        private void AddCopy()
        {
            int bookId = ConsoleHelper.ReadInt("ID of the book to add a copy to: ");

            ConsoleHelper.TryRun(() =>
            {
                BookCopy copy = new(bookId);
                _bookService.AddCopy(copy, bookId);
                ConsoleHelper.PrintSuccess($"Copy added (GUID: {copy.Id}).");
            });
        }

        private void DeleteBook()
        {
            int id = ConsoleHelper.ReadInt("ID of the book to delete: ");

            if (ConsoleHelper.TryRun(() => _bookService.DeleteBook(id)))
                ConsoleHelper.PrintSuccess("Book deleted.");
        }

        private void DeleteCopy()
        {
            Guid id = ConsoleHelper.ReadGuid("GUID of the copy to delete: ");

            if (ConsoleHelper.TryRun(() => _bookService.DeleteCopy(id)))
                ConsoleHelper.PrintSuccess("Copy deleted.");
        }

        private void ShowPendingRequests()
        {
            ConsoleHelper.TryRun(() =>
            {
                List<BorrowRequest>? requests = _borrowService.GetPendingRequests();
                if (requests == null || requests.Count == 0)
                {
                    Console.WriteLine("There are no pending requests.");
                    return;
                }

                foreach (BorrowRequest r in requests)
                    Console.WriteLine($"#{r.Id} | user #{r.UserId} | book #{r.BookId} | requested: {r.RequestDate:yyyy-MM-dd}");
            });
        }

        private void ApproveRequest()
        {
            int id = ConsoleHelper.ReadInt("ID of the request to approve: ");

            if (ConsoleHelper.TryRun(() => _borrowService.ApproveRequest(id)))
                ConsoleHelper.PrintSuccess("Request approved, a borrow record was created.");
        }

        private void RejectRequest()
        {
            int id = ConsoleHelper.ReadInt("ID of the request to reject: ");

            if (ConsoleHelper.TryRun(() => _borrowService.RejectRequest(id)))
                ConsoleHelper.PrintSuccess("Request rejected.");
        }

        private void PromoteToAdmin()
        {
            int userId = ConsoleHelper.ReadInt("ID of the user to promote: ");

            if (ConsoleHelper.TryRun(() => _userService.PromoteToAdmin(userId)))
                ConsoleHelper.PrintSuccess("User promoted to Admin.");
        }

        private void ShowStats()
        {
            ConsoleHelper.TryRun(() =>
            {
                Console.WriteLine($"Total books: {_bookService.GetTotalBookCount()}");
                Console.WriteLine($"Total copies: {_bookService.GetTotalCopieCount()}");
            });
        }
    }
}
