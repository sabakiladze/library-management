using LibraryManagementSystem.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task InitializeAsync();

        List<User>? GetAll();
        User? GetUserById(int id);
        List<User>? GetUserByName(string name);
        User? GetUserByEmail(string email);

        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task AddAsync(User user);
    }
}
