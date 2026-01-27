using Core.Interface;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstraction
{
    public interface IRepositoryStudent : IRepository<Student>
    {
        void InsertStudent(Student student);
    }
}
