using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAL.Abstraction
{
    public interface IRepositoryActivity
    {
        Activity? Get(int id);
        List<Activity> GetAll();
        List<Activity> GetByUserId(int userId);
        void Add(Activity activity);
        void Update(Activity activity);
        void Delete(int id);
    }
}
