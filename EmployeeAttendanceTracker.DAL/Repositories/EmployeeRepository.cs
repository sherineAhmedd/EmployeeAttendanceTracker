using EmployeeAttendanceTracker.DAL.Context;
using EmployeeAttendanceTracker.DAL.Data.Entities;
using EmployeeAttendanceTracker.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendanceTracker.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.Include(e => e.Department).ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.Include(e => e.Department)
                                           .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp != null)
            {
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeEmployeeId = null)
        {
            return !await _context.Employees
                .AnyAsync(e => e.Email == email && (!excludeEmployeeId.HasValue || e.EmployeeId != excludeEmployeeId.Value));
        }

        //to generate next employee code
        public async Task<int> GenerateUniqueEmployeeCodeAsync()
        {
            int code;
            do
            {
                code = new Random().Next(1000, 9999);
            } while (await _context.Employees.AnyAsync(e => e.EmployeeCode == code));
            return code;
        }



    }
}
