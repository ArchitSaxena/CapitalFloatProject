using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapitalFloatProject.Models
{
    public class StudentInfo
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public Int64 PhoneNo { get; set; }
        public string Email { get; set; }
        public string FatherName { get; set; }
        public string Address { get; set; }
        public string Course { get; set; }
        public DateTime DOB { get; set; }
        public string City { get; set; }
    }
}
