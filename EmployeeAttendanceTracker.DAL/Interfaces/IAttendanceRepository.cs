using EmployeeAttendanceTracker.DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceTracker.DAL.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<Attendance> GetByIdAsync(int id);
        Task<Attendance> GetAttendanceWithEmployeeAsync(int id);
        Task<Attendance> GetByEmployeeAndDateAsync(int employeeId, DateTime date);
        Task AddAsync(Attendance attendance);
        Task UpdateAsync(Attendance attendance);
        Task DeleteAsync(int id);
        
        Task<bool> ExistsForEmployeeOnDateAsync(int employeeId, DateTime date);
        Task<IEnumerable<Attendance>> FilterAsync(int? deptId, int? empId, DateTime? from, DateTime? to);
    }
}
