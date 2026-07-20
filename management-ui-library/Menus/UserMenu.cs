using Application.Interfaces.Services;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using management_ui_library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static LibraryManagementSystem.Domain.Enums.BookStatus;

namespace management_ui_library.Menus
{
    public class UserMenu
    {
        private readonly IBookService _bookService;
        private readonly IBorrowService _borrowService;
        private readonly IAuthService _authService;
        private readonly UserSession _userSession;

        public UserMenu(
            IBookService bookService,
            IBorrowService borrowService,
            IAuthService authService,
            UserSession userSession)
        {
            _bookService = bookService;
            _borrowService = borrowService;
            _authService = authService;
            _userSession = userSession;
        }

        public void Run()
        {
            while (_userSession.IsLoggedIn)
            {
                User current = _userSession.CurrentUser!;

                Console.WriteLine($"\n===== Client menu ({current.UserName}) =====");
                if (current.Fee is decimal fee && fee > 0)
                    ConsoleHelper.PrintError($"You have an outstanding fee: {fee:0.00}. New requests are blocked until an admin clears it.");

                Console.WriteLine("1. Full book catalog");
                Console.WriteLine("2. Search by title");
                Console.WriteLine("3. Search by author");
                Console.WriteLine("4. Search by publication year");
                Console.WriteLine("5. Request a book (borrow request)");
                Console.WriteLine("6. My requests");
                Console.WriteLine("7. Cancel a request (while still Pending)");
                Console.WriteLine("8. My active/past borrow records");
                Console.WriteLine("9. Return a book");
                Console.WriteLine("0. Log out");

                string choice = ConsoleHelper.ReadLineOrEmpty("Choose: ");

                switch (choice)
                {
                    case "1": ShowAllBooks(); break;
                    case "2": SearchByName(); break;
                    case "3": SearchByAuthor(); break;
                    case "4": SearchByYear(); break;
                    case "5": RequestBook(); break;
                    case "6": ShowMyRequests(); break;
                    case "7": CancelRequest(); break;
                    case "8": ShowMyRecords(); break;
                    case "9": ReturnBook(); break;
                    case "0": _authService.LogOut(); break;
                    default: ConsoleHelper.PrintError("Invalid menu option."); break;
                }
            }
        }

        private void ShowAllBooks()
        {
            PrintBooks(_bookService.GetAllBooks());
        }

        private void SearchByName()
        {
            string name = ConsoleHelper.ReadNonEmpty("Title: ");
            ConsoleHelper.TryRun(() => PrintBooks(_bookService.GetBooksByName(name)));
        }

        private void SearchByAuthor()
        {
            string first = ConsoleHelper.ReadNonEmpty("Author's first name: ");
            string last = ConsoleHelper.ReadNonEmpty("Author's last name: ");

            ConsoleHelper.TryRun(() =>
            {
                Author author = new Author { FirstName = first, LastName = last };
                PrintBooks(_bookService.GetBooksByAuthor(author));
            });
        }

        private void SearchByYear()
        {
            int year = ConsoleHelper.ReadInt("Publication year: ");
            ConsoleHelper.TryRun(() => PrintBooks(_bookService.GetBooksByPublishedYear(year)));
        }

        private void PrintBooks(List<Book> books)
        {
            if (books == null || books.Count == 0)
            {
                Console.WriteLine("No books found.");
                return;
            }

            foreach (Book b in books)
            {
                int available = b.Copies.Count(c => c.Status == Book_Status.Available);
                Console.WriteLine($"#{b.Id} | {b.Title} — {b.Author.FirstName} {b.Author.LastName} ({b.PublicationYear}) | available copies: {available}/{b.Copies.Count}");
            }
        }

        private void RequestBook()
        {
            int bookId = ConsoleHelper.ReadInt("Book ID (see catalog): ");

            if (ConsoleHelper.TryRun(() => _borrowService.RequestBook(bookId)))
                ConsoleHelper.PrintSuccess("Request sent. Wait for admin approval.");
        }

        private void ShowMyRequests()
        {
            ConsoleHelper.TryRun(() =>
            {
                List<BorrowRequest>? requests = _borrowService.GetMyRequests();
                if (requests == null || requests.Count == 0)
                {
                    Console.WriteLine("You have no requests.");
                    return;
                }

                foreach (BorrowRequest r in requests)
                    Console.WriteLine($"#{r.Id} | book #{r.BookId} | status: {r.Status} | requested: {r.RequestDate:yyyy-MM-dd}");
            });
        }

        private void CancelRequest()
        {
            int requestId = ConsoleHelper.ReadInt("ID of the request to cancel: ");

            if (ConsoleHelper.TryRun(() => _borrowService.CancelRequest(requestId)))
                ConsoleHelper.PrintSuccess("Request cancelled.");
        }

        private void ShowMyRecords()
        {
            ConsoleHelper.TryRun(() =>
            {
                List<BorrowRecord>? records = _borrowService.GetMyRecords();
                if (records == null || records.Count == 0)
                {
                    Console.WriteLine("Your borrow history is empty.");
                    return;
                }

                foreach (BorrowRecord r in records)
                {
                    string status = r.IsReturned
                        ? $"returned {r.ActualReturnDate:yyyy-MM-dd}"
                        : (r.IsOverdue ? "OVERDUE!" : "active");

                    Console.WriteLine($"#{r.Id} | book #{r.BookId} | due: {r.DueDate:yyyy-MM-dd} | {status}");
                }
            });
        }

        private void ReturnBook()
        {
            int recordId = ConsoleHelper.ReadInt("Record ID (see my records): ");

            if (ConsoleHelper.TryRun(() => _borrowService.ReturnBook(recordId)))
                ConsoleHelper.PrintSuccess("Book returned.");
        }
    }
}
