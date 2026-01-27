using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Notification
    {
        public int IdNotification { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
