using Domain.Exceptions;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LibraryManagementSystem.Domain.Enums.UserRole;

namespace Application.Validations
{
    public class Validation
    {
        public void EnsureId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
        }


        public void EnsureInput(object input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
        }


        public void EnsureString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be empty");
        }
        public void EnsureAdmin(UserSession current)
        {
            if (current.CurrentUser == null ||
                current.CurrentUser.Role!= Role.Admin)
            {
                throw new RoleException();
            }
        }

        public void EnsureLoggedIn(UserSession current)
        {
            if (current.CurrentUser == null)
            {
                throw new RoleException("first you have to log in system.");
            }
        }

    }
}
