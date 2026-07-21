using Application.Interfaces.Services;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using management_ui_library.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task Run()
        {
            while (_userSession.IsLoggedIn)
            {
                User current = _userSession.CurrentUser!;

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
                Console.WriteLine("11. My fee");
                Console.WriteLine("12. Delete my account");
                Console.WriteLine("0. Log out");
                Console.WriteLine("(tip: type 'cancel' at any prompt to back out of a multi-step action)");

                string choice = ConsoleHelper.ReadLineOrEmpty("Choose: ");

                switch (choice)
                {
                    case "1": ShowAllBooks(); break;
                    case "2": await AddBook(); break;
                    case "3": await AddCopy(); break;
                    case "4": await DeleteBook(); break;
                    case "5": await DeleteCopy(); break;
                    case "6": ShowPendingRequests(); break;
                    case "7": await ApproveRequest(); break;
                    case "8": await RejectRequest(); break;
                    case "9": await PromoteToAdmin(); break;
                    case "10": ShowStats(); break;
                    case "11": ShowMyFee(); break;
                    case "12": await DeleteAccount(); break;
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

        private async Task AddBook()
        {
            await ConsoleHelper.RunWithRetryAsync(async () =>
            {
                string title = ConsoleHelper.ReadNonEmpty("Title: ");
                string first = ConsoleHelper.ReadNonEmpty("Author's first name: ");
                string last = ConsoleHelper.ReadNonEmpty("Author's last name: ");
                int year = ConsoleHelper.ReadInt("Publication year: ");

                Console.WriteLine($"\nAbout to add: \"{title}\" by {first} {last} ({year})");
                if (!ConsoleHelper.ReadTypedConfirmation("Type YES to confirm, anything else to cancel: "))
                    throw new OperationCancelledByUserException();

                Author author = new() { FirstName = first, LastName = last };
                Book book = new(title, author, year);
                await _bookService.AddBookAsync(book);
                ConsoleHelper.PrintSuccess($"Book added (ID: {book.Id}).");
            });
        }

        private async Task AddCopy()
        {
            await ConsoleHelper.RunWithRetryAsync(async () =>
            {
                int bookId = ConsoleHelper.ReadInt("ID of the book to add a copy to: ");

                BookCopy copy = new(bookId);
                await _bookService.AddCopyAsync(copy, bookId);
                ConsoleHelper.PrintSuccess($"Copy added (GUID: {copy.Id}).");
            });
        }

        private async Task DeleteBook()
        {
            await ConsoleHelper.RunWithRetryAsync(async () =>
            {
                int id = ConsoleHelper.ReadInt("ID of the book to delete: ");
                await _bookService.DeleteBookAsync(id);
                ConsoleHelper.PrintSuccess("Book deleted.");
            });
        }

        private async Task DeleteCopy()
        {
            await ConsoleHelper.RunWithRetryAsync(async () =>
            {
                Guid id = ConsoleHelper.ReadGuid("GUID of the copy to delete: ");
                await _bookService.DeleteCopyAsync(id);
                ConsoleHelper.PrintSuccess("Copy deleted.");
            });
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

        private async Task ApproveRequest()
        {
            await ConsoleHelper.RunWithRetryAsync(async () =>
            {
                int id = ConsoleHelper.ReadInt("ID of the request to approve: ");
                await _borrowService.ApproveRequestAsync(id);
                ConsoleHelper.PrintSuccess("Request approved, a borrow record was created.");
            });
        }

        private async Task RejectRequest()
        {
            await ConsoleHelper.RunWithRetryAsync(async () =>
            {
                int id = ConsoleHelper.ReadInt("ID of the request to reject: ");
                await _borrowService.RejectRequestAsync(id);
                ConsoleHelper.PrintSuccess("Request rejected.");
            });
        }

        private async Task PromoteToAdmin()
        {
            await ConsoleHelper.RunWithRetryAsync(async () =>
            {
                int userId = ConsoleHelper.ReadInt("ID of the user to promote: ");
                await _userService.PromoteToAdminAsync(userId);
                ConsoleHelper.PrintSuccess("User promoted to Admin.");
            });
        }

        private void ShowStats()
        {
            ConsoleHelper.TryRun(() =>
            {
                Console.WriteLine($"Total books: {_bookService.GetTotalBookCount()}");
                Console.WriteLine($"Total copies: {_bookService.GetTotalCopieCount()}");
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
