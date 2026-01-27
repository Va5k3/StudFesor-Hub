using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Student
    {
        public int IdStudent { get; set; }
        public string? StudIndex { get; set; }
        public int IdUser { get; set; }
        
        public int Year { get; set; }
        public string? Major { get; set; }
    }
}
