using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Exceptions;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;

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


        public void LogIn(string email, string password)
        {
            User user = _userRepository.GetUserByEmail(email)
                ?? throw new AccountByThisEmailDoNotExists();


            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new IncorrectPasswordException();


            if (!user.IsEmailVerified)
                throw new EmailNotVerified("Please verify your email first");


            _userSession.CurrentUser = user;


            LogInLog log = new(email);

            List<LogInLog> logs = _logFileRepository.GetAllLine();

            logs.Add(log);

            _logFileRepository.SaveAll(logs);


            Console.WriteLine("Successfully logged in");
        }



        public void LogOut()
        {
            _userSession.CurrentUser = null;

            Console.WriteLine("Successfully logged out");
        }





        public void SignUp(string username, string email, string password)
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


            _userRepository.Add(user);


            _emailService.SendVerificationCode(
                user.Email,
                user.VerificationCode!
            );


            Console.WriteLine("Signed up successfully. Check your email.");
        }



        public void VerifyEmail(string email, string code)
        {
            User user = _userRepository.GetUserByEmail(email)
                ?? throw new AccountByThisEmailDoNotExists();


            if (!user.IsVerificationCodeValid(code))
                throw new Exception("Wrong or expired verification code");


            user.VerifyEmail();


            _userRepository.Update(user);


            Console.WriteLine("Email verified successfully");
        }



        public void DeleteAccount()
        {
            User user = _userSession.CurrentUser
                ?? throw new UnauthorizedAccessException();


            _userRepository.Delete(user.Id);


            _userSession.CurrentUser = null;


            Console.WriteLine("Account deleted successfully");
        }

       
    }
}