using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapitalFloatProject.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public int PersonID { get; set; }
        public string Course { get; set; }
        public DateTime DOB { get; set; }
    }
}
