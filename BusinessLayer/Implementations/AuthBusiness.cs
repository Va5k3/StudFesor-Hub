using System;
using BusinessLayer.Abstractions;
using BusinessLayer.DTOs;
using DAL.Abstraction;
using Entities;

namespace BusinessLayer.Implementations
{
    public class AuthBusiness : IAuthBusiness
    {
        private readonly IRepositoryUser _userRepo;

        public AuthBusiness(IRepositoryUser userRepo)
        {
            _userRepo = userRepo;
        }

        public UserDTO? Authenticate(string email, string password)
        {
            var user = _userRepo.GetAll().FirstOrDefault(u => u.Email == email && u.PasswordHash == password);

            if (user == null) return null;

            return new UserDTO
            {
                Id = user.IdUser,
                RoleId = user.IdRole,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                RoleName = user.IdRole == 1 ? "Admin" : (user.IdRole == 2 ? "Profesor" : "Student")
            };
        }

        public bool Register(User user)
        {
            var exists = _userRepo.GetAll().Any(u => u.Email == user.Email);
            if (exists) return false;

            _userRepo.Add(user);
            return true;
        }
    }
}