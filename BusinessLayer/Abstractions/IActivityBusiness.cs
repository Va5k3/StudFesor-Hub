using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstractions
{
    public interface IActivityBusiness
    {
        void Add(ActivityDTO activity, int userId);
        List<ActivityDTO> GetByUser(int userId);
    }
}
