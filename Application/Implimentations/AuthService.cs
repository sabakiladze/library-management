using Domain.Exceptions;
using LibraryManagementSystem.Domain.Models;
using LibraryManagementSystem.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;


namespace LibraryManagementSystem.Services.AuthService
{
    public class AuthService(IUserRepository userrepository, UserSession usersession, IFileRepository<LogInLog> logRepo, IFileRepository<User> sign) : IAuthService
    {
        private readonly IUserRepository _userrepository = userrepository;
        private readonly UserSession _userSession = usersession;
        private readonly IFileRepository<LogInLog> _logFileRepository = logRepo;
        private readonly IFileRepository<User> _signFileRepository = sign;

        public void LogIn(string email, string password)
        {
            User ? user = _userrepository.GetUserByEmail(email) ?? throw new AccountByThisEmailDoNotExists();
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new IncorrectPasswordException();

            _userSession.CurrentUser = user;
            LogInLog log = new LogInLog(email);
            List<LogInLog> allLogs = _logFileRepository.GetAllLine();
            allLogs.Add(log); 
            _logFileRepository.SaveAll(allLogs);

            Console.WriteLine("succesfully loged out");

          
        }

        public void LogOut()
        {
            _userSession.CurrentUser = null;
            Console.WriteLine("succesfully loged out");
        }

        public void SignUp(string username, string email, string password)
        {
            if (_userrepository.GetUserByEmail(email) != null)
                throw new EmailAlreadyIsInUseException();
            string passwordhash = BCrypt.Net.BCrypt.HashPassword(password);
            User user = new(username, email, passwordhash);


            List<User> users=_signFileRepository.GetAllLine();
            users.Add(user);
            _signFileRepository.SaveAll(users);

            Console.WriteLine("signed up successfully");

            //json
        }
    }
}
