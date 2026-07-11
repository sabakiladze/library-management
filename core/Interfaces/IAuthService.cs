using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DataAccess.Interfaces
{
    public interface IAuthService
    {
        void SignUp(string username, string email, string password);
        void LogIn(string email, string password);
        void LogOut();
    }
}
