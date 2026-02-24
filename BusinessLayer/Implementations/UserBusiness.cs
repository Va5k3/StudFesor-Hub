using BusinessLayer.Abstractions;
using BusinessLayer.DTOs;
using DAL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IRepositoryUser _userRepo;

        public UserBusiness(IRepositoryUser userRepo)
        {
            _userRepo = userRepo;
        }
        public bool Delete(int id)
        {
            _userRepo.Delete(id);
            return true;
        }

        public UserDTO? GetById(int id)
        {
            var user = _userRepo.Get(id);
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.IdUser,
                RoleId = user.IdRole,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                Email = user.Email ?? "",
                RoleName = user.IdRole == 2 ? "Profesor" : "Student"
            };
        }

        public bool Update(UserDTO userDto)
        {
            var dbUser = _userRepo.Get(userDto.Id);
            if (dbUser == null) return false;

            dbUser.FirstName = userDto.FirstName;
            dbUser.LastName = userDto.LastName;
            dbUser.Email = userDto.Email;

            _userRepo.Update(dbUser);
            return true;
        }
    }
}
