using EmployeeAttendanceTracker.DAL.Context;
using EmployeeAttendanceTracker.DAL.Data.Entities;
using EmployeeAttendanceTracker.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceTracker.DAL.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;


        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Attendance> GetByIdAsync(int id)
        {
            return await _context.Attendances.FindAsync(id);
        }

        public async Task<Attendance> GetAttendanceWithEmployeeAsync(int id)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                    .ThenInclude(e => e.Department)
                .FirstOrDefaultAsync(a => a.AttendanceId == id);
        }
        public async Task AddAsync(Attendance attendance) 
        {
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int attendanceId)
        {
            var attendance = await _context.Attendances.FindAsync(attendanceId);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsForEmployeeOnDateAsync(int employeeId, DateTime date)
        {
            return await _context.Attendances
                .AnyAsync(a => a.EmployeeId == employeeId && a.Date.Date == date.Date);
        }

        public async Task<Attendance> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date.Date == date.Date);
        }

        public async Task<IEnumerable<Attendance>> FilterAsync(int? deptId, int? empId, DateTime? from, DateTime? to)
        {
            // Filter attendance records based on provided criteria
            var query = _context.Attendances
                .Include(a => a.Employee)
                    .ThenInclude(e => e.Department)
                .AsQueryable();

            if (deptId.HasValue)
                query = query.Where(a => a.Employee.DepartmentId == deptId);

            if (empId.HasValue)
                query = query.Where(a => a.EmployeeId == empId);

            if (from.HasValue)
                query = query.Where(a => a.Date >= from.Value);

            if (to.HasValue)
                query = query.Where(a => a.Date <= to.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                    .ThenInclude(e => e.Department)
                .ToListAsync();
        }
    }
}
