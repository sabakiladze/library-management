using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        List<User> ? GetAll();
        User ? GetUserById(int id);
        List<User> ? GetUserByName(string name);
        User ? GetUserByEmail(string email);
        void Update(User user);
        
        void Delete(int id);
        void Add(User user);
    }
}
