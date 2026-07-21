using System;
using System.Threading.Tasks;

namespace management_ui_library.Utils
{
    public static class ConsoleHelper
    {
        private const string CancelKeyword = "cancel";

        private static void ThrowIfCancelled(string input)
        {
            if (input.Trim().Equals(CancelKeyword, StringComparison.OrdinalIgnoreCase))
                throw new OperationCancelledByUserException();
        }

        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";
                ThrowIfCancelled(input);

                if (int.TryParse(input.Trim(), out int value))
                    return value;

                PrintError("Please enter a whole number (or type 'cancel').");
            }
        }

        public static Guid ReadGuid(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine() ?? "").Trim();
                ThrowIfCancelled(input);

                if (Guid.TryParse(input, out Guid value))
                    return value;

                PrintError("Please enter a valid GUID (or type 'cancel').");
            }
        }

        public static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine() ?? "").Trim();
                ThrowIfCancelled(input);

                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                PrintError("This field can't be empty (or type 'cancel').");
            }
        }

        public static string ReadLineOrEmpty(string prompt)
        {
            Console.Write(prompt);
            return (Console.ReadLine() ?? "").Trim();
        }

        public static bool ReadTypedConfirmation(string prompt, string requiredWord = "YES")
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? "").Trim();
            return input == requiredWord;
        }

        public static void PrintError(string message)
        {
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"⚠ {message}");
            Console.ForegroundColor = previous;
        }

        public static void PrintSuccess(string message)
        {
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✔ {message}");
            Console.ForegroundColor = previous;
        }

        public static bool TryRun(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
                return false;
            }
        }

        public static async Task<bool> TryRunAsync(Func<Task> action)
        {
            try
            {
                await action();
                return true;
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Runs a full input-and-submit flow. On success, returns normally.
        /// If the user typed 'cancel' or declined a confirmation, stops
        /// quietly. On any other failure (bad input reaching a business
        /// rule, e.g. "book already exists"), prints the error and asks
        /// whether to try the whole thing again or give up — instead of
        /// silently dumping everything the user typed and returning to
        /// the menu.
        /// </summary>
        public static async Task RunWithRetryAsync(Func<Task> action)
        {
            while (true)
            {
                try
                {
                    await action();
                    return;
                }
                catch (OperationCancelledByUserException)
                {
                    Console.WriteLine("Cancelled — nothing was saved.");
                    return;
                }
                catch (Exception ex)
                {
                    PrintError(ex.Message);
                    Console.WriteLine("1. Try again");
                    Console.WriteLine("2. Cancel and return to menu");

                    string choice = ReadLineOrEmpty("Choose: ");
                    if (choice != "1")
                    {
                        Console.WriteLine("Cancelled — nothing was saved.");
                        return;
                    }
                    // loop: re-run 'action' from the top, re-prompting every field
                }
            }
        }
    }
}
