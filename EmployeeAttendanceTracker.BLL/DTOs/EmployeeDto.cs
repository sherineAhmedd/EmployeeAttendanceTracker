using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceTracker.BLL.DTOs
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; } 
        public int EmployeeCode { get; set; }
        public string? DepartmentName { get; set; } 
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public double AttendancePercentage { get; set; }
    }
}
