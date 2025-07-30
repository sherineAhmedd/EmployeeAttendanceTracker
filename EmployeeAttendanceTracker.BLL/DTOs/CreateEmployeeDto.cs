using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceTracker.BLL.DTOs
{
    public class CreateEmployeeDto
    {
        public int? EmployeeId { get; set; } // Nullable for Add/Edit
        
        [Required(ErrorMessage = "Please select a department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [RegularExpression(@"^\s*([A-Za-z]{2,}\s){3}[A-Za-z]{2,}\s*$",
            ErrorMessage = "Full Name must contain exactly 4 names, each at least 2 characters.")]
        public string? FullName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public int EmployeeCode { get; set; }
    }
}
