using LibraryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using static LibraryManagementSystem.Domain.Enums.UserRole;

namespace LibraryManagementSystem.Domain.Models
{
  
    public class User
    {
        private static int _idCounter = 0;
        public int Id { get; private set; }
        private string _userName = string.Empty;
        private string _email = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public decimal? Fee { get; set; } = 0;
        public string UserName
        {
            get => _userName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username can't be empty!");

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

        public Role Role { get; set; } = Role.Client;


        //public  User() { }

        public DateTime RegisterTime { get; set; } = DateTime.Now; 

        public User(string name, string email, string password)
        {

            UserName = name;
            Email = email;
           
            _idCounter++;
            Id = _idCounter;
            PasswordHash = password;
            if (Role == Role.Admin)
            {
                Fee = null;
            }
            else
            {
                Fee = 0;
            }

        }
       
        public  string ToJson(bool isloggedin)
        {
            var obj = new
            {
                Id,
                UserName,
                Email,
                PasswordHash,
                Role,
                Fee = Fee == null ? "Admin" : $"{Fee} GEL",
                IsLoggedIn=isloggedin

            };
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = false });
        }

    }
}
