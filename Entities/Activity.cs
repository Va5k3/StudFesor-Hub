using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Activity
    {
        public int IdActivity { get; set; } 
        public string Header { get; set; }
        public string Paragraph { get; set; }
        public string Type { get; set; }
        public DateTime Deadline { get; set; }
        public int CreatedUserId { get; set; }
    }
}
