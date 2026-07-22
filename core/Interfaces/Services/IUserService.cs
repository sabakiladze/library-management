using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task PromoteToAdminAsync(int userId);

        decimal GetMyFee();
    }
}
