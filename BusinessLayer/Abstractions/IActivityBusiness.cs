using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace BusinessLayer.Abstractions
{
    public interface IActivityBusiness
    {
        List<Activity> GetUserActivities(int userId);
        Activity? GetActivity(int id);
        bool CreateActivity(Activity activity);
        bool UpdateActivity(Activity activity);
        bool DeleteActivity(int id);
        List<Activity> GetUpcomingActivities(int userId);
        List<Activity> GetOverdueActivities(int userId);
    }
}
