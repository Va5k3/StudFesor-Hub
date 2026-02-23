using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Activity
    {
        public int IdActivity { get; set; } 
        public string Header { get; set; } = string.Empty;
        public string Paragraph { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }
        public int CreatedUserId { get; set; }
    }
}
