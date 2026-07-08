using Domain.Exceptions;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LibraryManagementSystem.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly List<User> _users;
        private User _current_user;

        public AuthService(List<User> users)
        {
            _users = users;
        }
        public User LogIn(string email, string password)
        {
            User user = _users.FirstOrDefault(x => x.Email == email);
            if (user==null)
            {
                throw new AccountByThisEmailDoNotExists();
            }
            if (user.PasswordHash != BCrypt.Net.BCrypt.HashPassword(password))
              throw new IncorrectPasswordException();
            _current_user = user;
            user.IsActive = true;
            Console.WriteLine("succesfully loged out");
            return user;
           

        }

        public void LogOut()
        {
            _current_user = null;
            _current_user.IsActive = false;
            Console.WriteLine("succesfully loged out");
        }

        public void SignUp(string username, string email, string password)
        {
            if (_users.Any(x => x.Email == email))
                throw new EmailAlreadyIsInUseException();
            string passwordhash = BCrypt.Net.BCrypt.HashPassword(password);
            User user = new User(username, email, passwordhash);
            _users.Add(user);
            Console.WriteLine("signed up successfully");
        }
    }
}
