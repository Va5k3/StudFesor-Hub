using Core.Interface;
using Entities;
using System.Collections.Generic;

namespace DAL.Abstraction
{
    public interface IRepositorySchedule : IRepository<Schedule>
    {
        void InsertSchedule(Schedule schedule);
        List<ScheduleDTO> GetAllDetailed();
        bool DeleteAll();
    }
}
