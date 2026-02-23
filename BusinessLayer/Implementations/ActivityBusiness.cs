using BusinessLayer.Abstractions;
using BusinessLayer.DTOs;
using Core.Interface;
using DAL.Abstraction;
using Entities;
using System.Reflection.PortableExecutable;

namespace BusinessLayer.Implementations
{
    public class ActivityBusiness : IActivityBusiness
    {
        private readonly IRepositoryActivity _activityRepo;
        public ActivityBusiness(IRepositoryActivity activityRepo)
        {
            _activityRepo = activityRepo;
        }

        public void Add(ActivityDTO activityDto, int userId)
        {
            var newActivity = new Activity
            {
                Header = activityDto.Header,
                Paragraph = activityDto.Paragraph,
                Type = activityDto.Type,
                Deadline = activityDto.Deadline,
                CreatedUserId = userId
            };

            _activityRepo.Add(newActivity);
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

        public List<ActivityDTO> GetByUser(int userId)
        {
            return _activityRepo.GetAll()
                .Where(a => a.CreatedUserId == userId)
                .Select(a => new ActivityDTO
                {
                    Header = a.Header,
                    Paragraph = a.Paragraph,
                    Type = a.Type,
                    Deadline = a.Deadline
                })
                .ToList();
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
            return _activityRepo.GetByUserId(userId); ;
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