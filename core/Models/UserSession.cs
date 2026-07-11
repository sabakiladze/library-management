using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Domain.Models;

namespace Domain.Models
{
    public class UserSession
    {
        public User CurrentUser { get; set; }
        public bool IsLoggedIn => CurrentUser != null;
    }
}
