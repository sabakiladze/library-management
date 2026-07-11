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
        public UserRepository(IFileRepository<User> file)
        {
           _fileRepository = file;
        }
        public List<User> GetAll() => _fileRepository.GetAllLine();

        public User GetUserByEmail(string email) => _fileRepository.GetAllLine().FirstOrDefault(x => x.Email == email);

        public User GetUserById(int id) => _fileRepository.GetAllLine().FirstOrDefault(x => x.Id == id);

        public List<User> GetUserByName(string name) => _fileRepository.GetAllLine().Where(x => x.UserName == name).ToList();


    }
}
