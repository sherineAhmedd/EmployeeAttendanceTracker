using EmployeeAttendanceTracker.BLL.DTOs;
using EmployeeAttendanceTracker.DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceTracker.BLL.ServiceInterfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesWithAttendanceAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<EmployeeDto?> GetEmployeeDtoByIdAsync(int id);
        Task AddEmployeeAsync(CreateEmployeeDto dto);
        Task UpdateEmployeeAsync(EmployeeDto dto);
        Task DeleteEmployeeAsync(int id);
        Task<Employee> GetEmployeeWithDepartmentByIdAsync(int id);

        Task<bool> EmployeeExistsAsync(int id);



    }
}
