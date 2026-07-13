using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces;
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
        }
        public List<User> ?GetAll() => _users;

        public User ? GetUserByEmail(string email) => _users.FirstOrDefault(x => x.Email == email);

        public User ? GetUserById(int id) => _users.FirstOrDefault(x => x.Id == id);

        public List<User>?  GetUserByName(string name) => _users.Where(x => x.UserName == name).ToList();


    }
}
