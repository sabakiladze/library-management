using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Exceptions;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Implimentations
{
    public class AuthService(
        IUserRepository userRepository,
        UserSession userSession,
        IFileRepository<LogInLog> logRepo,
        IEmailService emailService) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly UserSession _userSession = userSession;
        private readonly IFileRepository<LogInLog> _logFileRepository = logRepo;
        private readonly IEmailService _emailService = emailService;


        public async Task LogInAsync(string email, string password)
        {
            User user = _userRepository.GetUserByEmail(email)
                ?? throw new AccountByThisEmailDoNotExists();

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new IncorrectPasswordException();

            if (!user.IsEmailVerified)
                throw new EmailNotVerified("Please verify your email first");

            _userSession.CurrentUser = user;

            LogInLog log = new(email);

            List<LogInLog> logs = await _logFileRepository.GetAllLineAsync();

            logs.Add(log);

            await _logFileRepository.SaveAllAsync(logs);

            Console.WriteLine("Successfully logged in");
        }


        public void LogOut()
        {
            _userSession.CurrentUser = null;

            Console.WriteLine("Successfully logged out");
        }


        public async Task SignUpAsync(string username, string email, string password)
        {
            if (_userRepository.GetUserByEmail(email) != null)
                throw new EmailAlreadyIsInUseException();

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            User user = new(
                username,
                email,
                passwordHash
            );

            user.GenerateVerificationCode();

            await _userRepository.AddAsync(user);

            _emailService.SendVerificationCode(
                user.Email,
                user.VerificationCode!
            );

            Console.WriteLine("Signed up successfully. Check your email.");
        }


        public async Task VerifyEmailAsync(string email, string code)
        {
            User user = _userRepository.GetUserByEmail(email)
                ?? throw new AccountByThisEmailDoNotExists();

            if (!user.IsVerificationCodeValid(code))
                throw new Exception("Wrong or expired verification code");

            user.VerifyEmail();

            await _userRepository.UpdateAsync(user);

            Console.WriteLine("Email verified successfully");
        }


        public async Task DeleteAccountAsync()
        {
            User user = _userSession.CurrentUser
                ?? throw new UnauthorizedAccessException();

            await _userRepository.DeleteAsync(user.Id);

            _userSession.CurrentUser = null;

            Console.WriteLine("Account deleted successfully");
        }
    }
}
