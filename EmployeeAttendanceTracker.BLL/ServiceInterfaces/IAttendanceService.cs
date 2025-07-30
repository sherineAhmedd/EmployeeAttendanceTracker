using EmployeeAttendanceTracker.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceTracker.BLL.ServiceInterfaces
{
    public interface IAttendanceService
    {
        Task<List<CreateAttendanceDto>> FilterAttendancesAsync(int? deptId, int? empId, DateTime? from, DateTime? to);
        Task AddAttendanceAsync(CreateAttendanceDto dto);
        Task UpdateAttendanceAsync(CreateAttendanceDto dto);
        Task DeleteAttendanceAsync(int attendanceId);
        Task<CreateAttendanceDto> GetByIdAsync(int id);
        Task<CreateAttendanceDto> GetByEmployeeAndDateAsync(int employeeId, DateTime date);
    }
}
