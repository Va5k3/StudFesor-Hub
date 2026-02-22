using BusinessLayer.Abstractions;
using DAL.Abstraction;
using Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Implementations
{
    public class ScheduleBusiness : IScheduleBusiness
    {
        private readonly IRepositorySchedule _scheduleRepo;

        public ScheduleBusiness(IRepositorySchedule scheduleRepo)
        {
            _scheduleRepo = scheduleRepo;
        }

        public List<ScheduleDTO> GetDetailedSchedule()
        {
            return _scheduleRepo.GetAllDetailed();
        }

        public bool ValidateAndAdd(Schedule schedule)
        {
            if (schedule.Time == null || string.IsNullOrEmpty(schedule.Day))
                return false;

            var exists = _scheduleRepo.GetAll().Any(x =>
                x.Cabinet == schedule.Cabinet &&
                x.Day == schedule.Day &&
                x.Time == schedule.Time);

            if (exists) return false;

            _scheduleRepo.Add(schedule);
            return true;
        }
    }
}