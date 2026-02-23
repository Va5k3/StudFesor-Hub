using BusinessLayer.Abstractions;
using BusinessLayer.DTOs;
using Core.Interface;
using DAL.Abstraction;
using Entities;

namespace BusinessLayer.Implementations
{
    public class ActivityBusiness : IActivityBusiness
    {
        private readonly IRepositoryActivity _activityRepo;
        public ActivityBusiness(IRepositoryActivity activityRepo)
        {
            _activityRepo = activityRepo;
        }
        public bool CreateActivity(Activity activity)
        {
            try
            {
                _activityRepo.Add(activity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteActivity(int id)
        {
            try
            {
                _activityRepo.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Activity? GetActivity(int id)
        {
            return _activityRepo.Get(id); 
        }

        public List<Activity> GetOverdueActivities(int userId)
        {
            var activities = _activityRepo.GetByUserId(userId);
            return activities.Where(a => a.Deadline < DateTime.Now).ToList();
        }

        public List<Activity> GetUpcomingActivities(int userId)
        {
            var activities = _activityRepo.GetByUserId(userId);
            return activities.Where(a => a.Deadline >= DateTime.Now).ToList();
        }

        public List<Activity> GetUserActivities(int userId)
        {
            return _activityRepo.GetByUserId(userId);
        }

        public bool UpdateActivity(Activity activity)
        {
            try
            {
                _activityRepo.Update(activity);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}