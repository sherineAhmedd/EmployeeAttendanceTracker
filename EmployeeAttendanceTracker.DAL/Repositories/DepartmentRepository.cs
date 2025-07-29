using EmployeeAttendanceTracker.DAL.Context;
using EmployeeAttendanceTracker.DAL.Data.Entities;
using EmployeeAttendanceTracker.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;



namespace EmployeeAttendanceTracker.DAL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;


        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            var departments = await _context.Departments
       .Include(d => d.Employees) // Include employees for counting
       .ToListAsync();

            return departments;

        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            return !await _context.Departments
                .AnyAsync(d => d.DepartmentName == name && (excludeId == null || d.DepartmentId != excludeId));
        }

        public async Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null)
        {
            return !await _context.Departments
                .AnyAsync(d => d.DepartmentCode == code && (excludeId == null || d.DepartmentId != excludeId));
        }

        public async Task<IEnumerable<Department>> GetForDropdownAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }





    }
}
