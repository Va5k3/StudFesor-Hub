using System;

namespace BusinessLayer.DTOs
{
    public class ScheduleDTO
    {
        public int Id { get; set; }
        public string Day { get; set; } = string.Empty;
        public string TimeDisplay { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string ProfessorName { get; set; } = string.Empty;
        public int Cabinet { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}