using BusinessLayer.Abstractions;
using BusinessLayer.DTOs;
using Core.Interface;
using DAL.Abstraction;
using Entities;

namespace BusinessLayer.Implementations
{
    public class ActivityBusiness : IActivityBusiness
    {
        private readonly IRepository<Activity> _activityRepo;

        public ActivityBusiness(IRepository<Activity> activityRepo)
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
    }
}