using Application.Interfaces;
using Application.Validations;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LibraryManagementSystem.Domain.Enums.UserRole;

namespace Application.Implimentations
{
    public class UserService : IUserService
    {
        private readonly UserSession _userSession;
        private readonly IUserRepository _userRepository;
        private readonly Validation _validations;

        public UserService( UserSession userservice, IUserRepository userrepository, Validation validation)
        {
            _userRepository = userrepository;
            _userSession = userservice;
            _validations = validation;
        }
        public void PromoteToAdmin(int userId)
        {
            _validations.EnsureAdmin(_userSession);
            User? user = _userRepository.GetUserById(userId) ?? throw new UserNotFound();
            if(user.Role==Role.Admin) throw new UserIsAlreadyAdmin();
            user.PromoteToAdmin();
            _userRepository.Update(user);

        }
    }
}
