using EmployeeAttendanceTracker.BLL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EmployeeAttendanceTracker.BLL.DTOs
{
    public class CreateAttendanceDto
    {

        public int? AttendanceId { get; set; } // Nullable for Add/Edit
        public int EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public AttendanceStatus Status { get; set; }


        public string? EmployeeName { get; set; } // for display only
        public string? DepartmentName { get; set; } // for display only
    }
}
