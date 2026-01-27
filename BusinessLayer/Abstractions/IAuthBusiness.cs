using System;
using BusinessLayer.DTOs;

namespace BusinessLayer.Abstractions
{
    public interface IAuthBusiness
    {
        UserDTO? Authenticate(string email, string password);
        bool Register(Entities.User user);
    }
}