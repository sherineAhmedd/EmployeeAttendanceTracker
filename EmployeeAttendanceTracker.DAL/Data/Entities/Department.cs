using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceTracker.DAL.Data.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string DepartmentName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[A-Z]{4}$")]
        public string DepartmentCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        public ICollection<Employee>? Employees { get; set; } //1 to M relationShip
    }
}
