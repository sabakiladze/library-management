using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task PromoteToAdminAsync(int userId);

        /// <summary>Current logged-in user's outstanding fee. Throws if nobody is logged in.</summary>
        decimal GetMyFee();
    }
}
