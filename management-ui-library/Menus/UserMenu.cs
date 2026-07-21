using Application.Interfaces.Services;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using management_ui_library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LibraryManagementSystem.Domain.Enums.BookStatus;

namespace management_ui_library.Menus
{
    public class UserMenu
    {
        private readonly IBookService _bookService;
        private readonly IBorrowService _borrowService;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly UserSession _userSession;

        public UserMenu(
            IBookService bookService,
            IBorrowService borrowService,
            IAuthService authService,
            IUserService userService,
            UserSession userSession)
        {
            _bookService = bookService;
            _borrowService = borrowService;
            _authService = authService;
            _userService = userService;
            _userSession = userSession;
        }

        public async Task Run()
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
                Console.WriteLine("10. My fee");
                Console.WriteLine("11. Delete my account");
                Console.WriteLine("0. Log out");
                Console.WriteLine("(type 'cancel' at any time to back out of a multistep action)");

                string choice = ConsoleHelper.ReadLineOrEmpty("Choose: ");

                switch (choice)
                {
                    case "1": ShowAllBooks(); break;
                    case "2": SearchByName(); break;
                    case "3": SearchByAuthor(); break;
                    case "4": SearchByYear(); break;
                    case "5": await RequestBook(); break;
                    case "6": ShowMyRequests(); break;
                    case "7": await CancelRequest(); break;
                    case "8": ShowMyRecords(); break;
                    case "9": await ReturnBook(); break;
                    case "10": ShowMyFee(); break;
                    case "11": await DeleteAccount(); break;
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

        private async Task RequestBook()
        {
            await ConsoleHelper.RunWithRetryAsync(async () =>
            {
                int bookId = ConsoleHelper.ReadInt("Book ID (see catalog): ");
                await _borrowService.RequestBookAsync(bookId);
                ConsoleHelper.PrintSuccess("Request sent. Wait for admin approval.");
            });
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

        private async Task CancelRequest()
        {
            await ConsoleHelper.RunWithRetryAsync(async () =>
            {
                int requestId = ConsoleHelper.ReadInt("ID of the request to cancel: ");
                await _borrowService.CancelRequestAsync(requestId);
                ConsoleHelper.PrintSuccess("Request cancelled.");
            });
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
                    ///<summary>
                    ///ლამბდა ფუნქცია ჩერდება: return; ეუბნება ლამბდას: "შენი საქმე მორჩა, ქვემოთ არსებული foreach ციკლი აღარ გაუშვა".

                   /// მართვა უბრუნდება TryRun - ს: რადგან ლამბდა ფუნქციამ მუშაობა უშეცდომოდ(Exception - ის გარეშე) დაასრულა, TryRun გადადის თავისი კოდის მომდევნო ხაზზე.

                   /// TryRun - იც ჩერდება: TryRun - ის შიგნით ეშვება return true; — ეს უკვე თვითონ TryRun მეთოდს ხურავს.

                    /// მართვა უბრუნდება ShowMyRecords() - ს: რადგან TryRun-იც დაიხურა, ShowMyRecords() - იც სრულდება და მომხმარებელი ბრუნდება მთავარ მენიუში.
                    
                    ///</summary>
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

        private async Task ReturnBook()
        {
            await ConsoleHelper.RunWithRetryAsync(async () =>
            {
                int recordId = ConsoleHelper.ReadInt("Record ID (see my records): ");
                await _borrowService.ReturnBookAsync(recordId);
                ConsoleHelper.PrintSuccess("Book returned.");
            });
        }

        private void ShowMyFee()
        {
            ConsoleHelper.TryRun(() =>
            {
                decimal fee = _userService.GetMyFee();
                if (fee > 0)
                    ConsoleHelper.PrintError($"Your outstanding fee: {fee:0.00}");
                else
                    ConsoleHelper.PrintSuccess("You have no outstanding fee.");
            });
        }

        private async Task DeleteAccount()
        {
            ConsoleHelper.PrintError("This will permanently delete your account. This cannot be undone.");
            bool confirmed = ConsoleHelper.ReadTypedConfirmation("Type YES to confirm: ");

            if (!confirmed)
            {
                Console.WriteLine("Cancelled — your account was not deleted.");
                return;
            }

            if (await ConsoleHelper.TryRunAsync(() => _authService.DeleteAccountAsync()))
                ConsoleHelper.PrintSuccess("Your account has been deleted.");
        }
    }
}
