using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstractions
{
    public interface IUserBusiness
    {
        UserDTO? GetById(int id);
        bool Update(UserDTO user);
        bool Delete(int id);
    }
}
