using Application.Interfaces.Repositories;
using Domain.Exceptions;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IFileRepository<User> _fileRepository;
        private readonly List<User> _users;
        public UserRepository(IFileRepository<User> file)
        {
           _fileRepository = file;
            _users = _fileRepository.GetAllLine() ?? new List<User>();
            User.SyncIdCounter(_users);
        }
        public List<User> ?GetAll() => _users;

        public User? GetUserByEmail(string email) =>
            _users.FirstOrDefault(x =>
                x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public User ? GetUserById(int id) => _users.FirstOrDefault(x => x.Id == id);

        public List<User> GetUserByName(string name) =>
            _users.Where(x =>
                x.UserName.Equals(name, StringComparison.OrdinalIgnoreCase))
            .ToList();

        public void Update(User user)
        {
            int index = _users.FindIndex(x => x.Id == user.Id);

            if (index == -1)
                throw new UserNotFound();

            _users[index] = user;

            Save();

        }
        public void Delete(int id)
        {
            var user = GetUserById(id) ?? throw new UserNotFound();

            _users.Remove(user);

            Save();
        }
        public void Add(User user)
        {
            _users.Add(user);
            _fileRepository.SaveAll(_users);
        }
        private void Save()
        {
            _fileRepository.SaveAll(_users);
        }


    }
}
