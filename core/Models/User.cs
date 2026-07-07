using LibraryManagementSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LibraryManagementSystem.Core.Enums.UserRole;

namespace LibraryManagementSystem.Core.Models
{
   

    internal abstract class User
    {
       
        private string _userName = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;

        public int Id { get; set; } 

        public string UserName
        {
            get => _userName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Password can't be empty!");

                if (value.Length < 3 || value.Length > 50)
                    throw new ArgumentException("User must enter min 3 and max 50 charachters!");

                _userName = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email can't be empty!");

                if (!value.Contains('@') || !value.Contains('.'))
                    throw new ArgumentException("Format of email is invalid  (mustinclude '@' and  '.')!");

                _email = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("password can't be empty!");
                if (value.Length < 8)
                    throw new ArgumentException("password's length must be 8 or more");

                _password = value;
            }
        }

        public bool EmailVerified { get; set; } = false;
        public Role Role { get; set; } = Role.Client;

        protected User() { }

        protected User(string name, string email, string password)
        {
            UserName = name;
            Email = email;
            Password = password;
        }
    }
}
