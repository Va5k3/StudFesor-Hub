using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Profesor
    {
        public int IdProf { get; set; }
        public int IdUser { get; set; }
        public string? Section { get; set; }
        public string? Cabinet { get; set; }
    }
}
