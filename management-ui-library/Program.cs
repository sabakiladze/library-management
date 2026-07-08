using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Services.AuthService;


namespace management_ui_library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isRunning = true;
            User currentUser = null;
            List<User> userDatabase = new List<User>();
            var authService = new AuthService(userDatabase);

            while (isRunning)
            {
                if (currentUser == null)
                {
                    Console.WriteLine("\n--- WELCOME ---");
                    Console.WriteLine("1. LogIn");
                    Console.WriteLine("2. SignUp");
                    Console.WriteLine("3. Exit");
                    Console.Write("Select: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            bool loginSuccess = false;
                            while (!loginSuccess)
                            {
                                Console.Write("Email (or 'back'): ");
                                string email = Console.ReadLine();
                                if (email.ToLower() == "back") break;

                                Console.Write("Password: ");
                                string password = Console.ReadLine();

                                try
                                {
                                    currentUser = authService.LogIn(email, password);
                                    loginSuccess = true;
                                    Console.WriteLine("Successfully logged in!");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                            }
                            break;

                        case "2":
                            Console.Write("Username: ");
                            string username = Console.ReadLine();
                            Console.Write("Email: ");
                            string emailSignUp = Console.ReadLine();
                            Console.Write("Password: ");
                            string passwordSignUp = Console.ReadLine();

                            try
                            {
                                authService.SignUp(username, emailSignUp, passwordSignUp);
                                Console.WriteLine("Registration successful!");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;

                        case "3":
                            isRunning = false;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"\nWELCOME, {currentUser.UserName}!");
                    Console.WriteLine("1. See Catalog");
                    Console.WriteLine("2. LogOut");
                    Console.Write("Select: ");

                    string choice = Console.ReadLine();
                    if (choice == "2")
                    {
                        authService.LogOut();
                        currentUser = null;
                    }
                }
            }
        }
    }
}