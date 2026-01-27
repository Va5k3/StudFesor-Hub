using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Schedule
    {
        public  int? IdSchedule { get; set; }
        public int? IdProfesor { get; set; }
        public int? IdSubject{ get; set; }
        public  int? IdStudent { get; set; }
        public string? Day { get; set; }
        public TimeSpan? Time { get; set; }
        public string? Type { get; set; }
        public int? Cabinet { get; set; }
    }
}
