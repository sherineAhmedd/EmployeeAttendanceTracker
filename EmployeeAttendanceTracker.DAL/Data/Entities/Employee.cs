using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceTracker.DAL.Data.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [RegularExpression(@"^([A-Za-z]{2,}\s){3}[A-Za-z]{2,}$")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public int EmployeeCode { get; set; } //System-generated


        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }  //Fk
        public Department? Department { get; set; }


        public ICollection<Attendance>? Attendances { get; set; }



    }
}
