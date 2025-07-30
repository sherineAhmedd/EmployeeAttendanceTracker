using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceTracker.DAL.Data.Entities
{
    public class Attendance
    {
        public int AttendanceId { get; set; }

        public int EmployeeId { get; set; } //Fk
        public Employee? Employee { get; set; }

        public DateTime Date { get; set; }

       
        public string Status { get; set; } = string.Empty;




    }
}
