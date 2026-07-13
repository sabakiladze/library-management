using LibraryManagementSystem.Domain.Models;

namespace Domain.Models
{
    public class UserSession
    {
        public User? CurrentUser { get; set; }
        public bool IsLoggedIn => CurrentUser != null;
    }
}
