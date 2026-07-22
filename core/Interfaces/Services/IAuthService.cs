using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task SignUpAsync(string username, string email, string password);
        Task LogInAsync(string email, string password);
        void LogOut();
        Task DeleteAccountAsync();
        Task VerifyEmailAsync(string email, string code);
    }
}
