using System;
using BusinessLayer.DTOs;
using Entities;

namespace BusinessLayer.Abstractions
{
    public interface IScheduleBusiness
    {
        List<ScheduleDTO> GetDetailedSchedule();
        bool ValidateAndAdd(Schedule schedule);
    }
}