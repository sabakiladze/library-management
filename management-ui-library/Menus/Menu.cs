using Application.Interfaces.Services;
using Domain.Models;
using management_ui_library.Utils;
using System;
using System.Threading.Tasks;
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

        public async Task Run()
        {
            Console.WriteLine("=== Library Management System ===");

            bool exit = false;
            while (!exit)
            {
                if (_userSession.IsLoggedIn)
                {
                    await RouteToRoleMenu();
                    continue;
                }

                Console.WriteLine("\n1. Register");
                Console.WriteLine("2. Log in");
                Console.WriteLine("3. Verify email");
                Console.WriteLine("0. Exit");

                string choice = ConsoleHelper.ReadLineOrEmpty("Choose: ");

                switch (choice)
                {
                    case "1": await Register(); break;
                    case "2": await Login(); break;
                    case "3": await VerifyEmail(); break;
                    case "0": exit = true; break;
                    default: ConsoleHelper.PrintError("Invalid menu option."); break;
                }
            }

            Console.WriteLine("Goodbye!");
        }

        private async Task RouteToRoleMenu()
        {
            if (_userSession.CurrentUser!.Role == Role.Admin)
                await _adminMenu.Run();
            else
                await _userMenu.Run();
        }

        private async Task Register()
        {
            await ConsoleHelper.RunWithRetryAsync(async () =>
            {
                string username = ConsoleHelper.ReadNonEmpty("Username (or 'cancel'): ");
                string email = ConsoleHelper.ReadNonEmpty("Email (or 'cancel'): ");
                string password = ConsoleHelper.ReadNonEmpty("Password (or 'cancel'): ");

                await _authService.SignUpAsync(username, email, password);

                ConsoleHelper.PrintSuccess("Registration successful. A verification code has been sent to your email.");
            });
        }

        private async Task VerifyEmail()
        {
            await ConsoleHelper.RunWithRetryAsync(async () =>
            {
                string email = ConsoleHelper.ReadNonEmpty("Email (or 'cancel'): ");
                string code = ConsoleHelper.ReadNonEmpty("Verification code (or 'cancel'): ");

                await _authService.VerifyEmailAsync(email, code);
                ConsoleHelper.PrintSuccess("Email verified. You can now log in.");
            });
        }

        private async Task Login()
        {
            await ConsoleHelper.RunWithRetryAsync(async () =>
            {
                string email = ConsoleHelper.ReadNonEmpty("Email (or 'cancel'): ");
                string password = ConsoleHelper.ReadNonEmpty("Password (or 'cancel'): ");

                await _authService.LogInAsync(email, password);
            });
        }
    }
}
