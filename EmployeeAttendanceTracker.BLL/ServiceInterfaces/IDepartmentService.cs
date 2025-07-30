using EmployeeAttendanceTracker.BLL.DTOs;
using EmployeeAttendanceTracker.DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceTracker.BLL.ServiceInterfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto?> GetDepartmentByIdAsync(int id);
        Task AddDepartmentAsync(CreateDepartmentDto dto);

        Task<DepartmentDto?> GetDepartmentDtoByIdAsync(int id);

        Task UpdateDepartmentAsync(DepartmentDto dto);
        Task DeleteDepartmentAsync(int id);


    }
}
