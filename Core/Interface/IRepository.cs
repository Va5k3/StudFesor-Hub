using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interface
{
    public interface IRepository<T>
    { 
        //crud
        public bool Add(T item);
        T Get(int id);
        List<T> GetAll();
        bool Update(T item);
        bool Delete(int id);


    }
}
