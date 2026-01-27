using System;

using BusinessLayer.Abstractions;
using BusinessLayer.DTOs;
using DAL.Abstraction;
using Entities;

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
            var rawData = _scheduleRepo.GetAll();

            return rawData.Select(s => new ScheduleDTO
            {
                Id = s.IdSchedule ?? 0,
                Day = s.Day ?? "/",
                TimeDisplay = s.Time?.ToString(@"hh\:mm") ?? "00:00",
                Cabinet = s.Cabinet ?? 0,
                Type = s.Type ?? "/",
                SubjectName = "Predmet ID: " + (s.IdSubject?.ToString() ?? "N/A"),
                ProfessorName = "Profesor ID: " + (s.IdProfesor?.ToString() ?? "N/A")
            }).ToList();
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