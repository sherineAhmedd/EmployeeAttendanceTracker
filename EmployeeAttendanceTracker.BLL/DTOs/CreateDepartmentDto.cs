using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceTracker.BLL.DTOs
{
    public class CreateDepartmentDto
    {
        public int? DepartmentId { get; set; } 

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? DepartmentName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{4}$", ErrorMessage = "Department Code must be exactly 4 uppercase letters")]
        public string? DepartmentCode { get; set; }

        [Required]
        [StringLength(100)]
        public string? Location { get; set; }
    }
}
