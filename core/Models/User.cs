using LibraryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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



        public DateTime RegisterTime { get; set; } = DateTime.Now;



        [JsonConstructor]// რადგან პრივატე სეტ მაქვ, და მნიშვნელობის მინიჭება ფროფერთისთვის გარედან შეუძლებელია და მხოლოდ კონსტრუქტორითააა შესაძლებლი, დესერალიზაციის დროს ვეღარ მივანიჭებ მნიშვნელობას ფაილიდან წამოღებულ მნიშვნელობებს ველებას, ეს მეხმარება რომ რომ ველებს მივანიჭო მნიშვნელობა გარედან. მხოლოდ იმათ რომელიც კონსტრუქტორშია.
        public User(string name, string email, string password)
        {

            Id=++_idCounter;
            UserName = name;
            Email = email;
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
       
        
        public User()
        {
            
        }
        public static void SyncIdCounter(List<User> users)
        {
            int maxId = users.Select(x => x.Id).DefaultIfEmpty(0).Max();
            if(maxId>_idCounter)
            _idCounter = maxId; 
        }

    }
}
