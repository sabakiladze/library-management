using Application.Interfaces.Repositories;
using Domain.Exceptions;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IFileRepository<User> _fileRepository;
        private List<User> _users = new();

        public UserRepository(IFileRepository<User> file)
        {
            _fileRepository = file;
        }

        public async Task InitializeAsync()
        {
            _users = await _fileRepository.GetAllLineAsync() ?? new List<User>();
            User.SyncIdCounter(_users);
        }

        public List<User>? GetAll() => _users;

        public User? GetUserByEmail(string email) =>
            _users.FirstOrDefault(x =>
                x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public User? GetUserById(int id) => _users.FirstOrDefault(x => x.Id == id);

        public List<User> GetUserByName(string name) =>
            _users.Where(x =>
                x.UserName.Equals(name, StringComparison.OrdinalIgnoreCase))
            .ToList();

        public async Task UpdateAsync(User user)
        {
            int index = _users.FindIndex(x => x.Id == user.Id);

            if (index == -1)
                throw new UserNotFound();

            _users[index] = user;

            await SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = GetUserById(id) ?? throw new UserNotFound();

            _users.Remove(user);

            await SaveAsync();
        }

        public async Task AddAsync(User user)
        {
            _users.Add(user);
            await SaveAsync();
        }

        private Task SaveAsync() => _fileRepository.SaveAllAsync(_users);
    }
}
