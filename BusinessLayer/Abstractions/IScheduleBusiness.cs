using System;
using Entities;
using System.Collections.Generic;

namespace BusinessLayer.Abstractions
{
    public interface IScheduleBusiness
    {
        List<ScheduleDTO> GetDetailedSchedule();
        bool ValidateAndAdd(Schedule schedule);
    }
}