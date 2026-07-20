using System;

namespace management_ui_library.Utils
{
    public static class ConsoleHelper
    {
        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input.Trim(), out int value))
                    return value;

                PrintError("Please enter a whole number.");
            }
        }

        public static Guid ReadGuid(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine() ?? "").Trim();

                if (Guid.TryParse(input, out Guid value))
                    return value;

                PrintError("Please enter a valid GUID (the one shown next to the copy in the catalog).");
            }
        }

        public static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine() ?? "").Trim();

                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                PrintError("This field can't be empty.");
            }
        }

        public static string ReadLineOrEmpty(string prompt)
        {
            Console.Write(prompt);
            return (Console.ReadLine() ?? "").Trim();
        }







        public static void PrintError(string message)
        {
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($" {message}");
            Console.ForegroundColor = previous;
        }

        public static void PrintSuccess(string message)
        {
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" {message}");
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
    }
}
