using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstraction
{
    public interface IRepositoryUser
    {
        User? Get(int id);
        User? GetByEmail(string email);
        List<User> GetAll();
        void Add(User user);
        void Update(User user);
        void Delete(int id);
    }
}
