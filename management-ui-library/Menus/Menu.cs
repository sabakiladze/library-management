using Application.Interfaces.Services;
using Domain.Models;
using management_ui_library.Utils;
using System;
using static LibraryManagementSystem.Domain.Enums.UserRole;

namespace management_ui_library.Menus
{
    public class Menu
    {
        private readonly IAuthService _authService;
        private readonly UserSession _userSession;
        private readonly UserMenu _userMenu;
        private readonly AdminMenu _adminMenu;

        public Menu(
            IAuthService authService,
            UserSession userSession,
            UserMenu userMenu,
            AdminMenu adminMenu)
        {
            _authService = authService;
            _userSession = userSession;
            _userMenu = userMenu;
            _adminMenu = adminMenu;
        }

        public void Run()
        {
            Console.WriteLine("=== Library Management System ===");

            bool exit = false;
            while (!exit)
            {
                if (_userSession.IsLoggedIn)
                {
                    RouteToRoleMenu();
                    continue;
                }

                Console.WriteLine("\n1. Register");
                Console.WriteLine("2. Log in");
                Console.WriteLine("3. Verify email");
                Console.WriteLine("0. Exit");

                string choice = ConsoleHelper.ReadLineOrEmpty("Choose: ");

                switch (choice)
                {
                    case "1": Register(); break;
                    case "2": Login(); break;
                    case "3": VerifyEmail(); break;
                    case "0": exit = true; break;
                    default: ConsoleHelper.PrintError("Invalid menu option."); break;
                }
            }

            Console.WriteLine("Goodbye!");
        }

        private void RouteToRoleMenu()
        {
            if (_userSession.CurrentUser!.Role == Role.Admin)
                _adminMenu.Run();
            else
                _userMenu.Run();
        }

        private void Register()
        {
            string username = ConsoleHelper.ReadNonEmpty("Username: ");
            string email = ConsoleHelper.ReadNonEmpty("Email: ");
            string password = ConsoleHelper.ReadNonEmpty("Password: ");

            if (ConsoleHelper.TryRun(() => _authService.SignUp(username, email, password)))
            {
                ConsoleHelper.PrintSuccess("Registration successful. A verification code has been sent to your email.");
                Console.WriteLine("Note: if email isn't actually configured (in appsettings.json), sending the code will fail — contact the admin/developer.");
            }
        }

        private void VerifyEmail()
        {
            string email = ConsoleHelper.ReadNonEmpty("Email: ");
            string code = ConsoleHelper.ReadNonEmpty("Verification code: ");

            if (ConsoleHelper.TryRun(() => _authService.VerifyEmail(email, code)))
                ConsoleHelper.PrintSuccess("Email verified. You can now log in.");
        }

        private void Login()
        {
            string email = ConsoleHelper.ReadNonEmpty("Email: ");
            string password = ConsoleHelper.ReadNonEmpty("Password: ");

            ConsoleHelper.TryRun(() => _authService.LogIn(email, password));
        }
    }
}
