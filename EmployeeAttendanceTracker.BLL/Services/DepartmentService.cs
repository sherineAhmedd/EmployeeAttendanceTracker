using EmployeeAttendanceTracker.BLL.DTOs;
using EmployeeAttendanceTracker.BLL.ServiceInterfaces;
using EmployeeAttendanceTracker.DAL.Data.Entities;
using EmployeeAttendanceTracker.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttendanceTracker.BLL.Services
{
    public class DepartmentService :IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            //return await _departmentRepository.GetAllAsync();
            var departments = await _departmentRepository.GetAllAsync();

            return departments.Select(d => new DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                DepartmentCode = d.DepartmentCode,
                Location = d.Location,
                EmpCount = d.Employees.Count()

            }).ToList();
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }

        public async Task AddDepartmentAsync(Department department)
        {
            if (!await _departmentRepository.IsNameUniqueAsync(department.DepartmentName))
                throw new Exception("Department name must be unique.");

            if (!await _departmentRepository.IsCodeUniqueAsync(department.DepartmentCode))
                throw new Exception("Department code must be unique.");

            await _departmentRepository.AddAsync(department);
            await _departmentRepository.SaveChangesAsync();

        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            await _departmentRepository.UpdateAsync(department);
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            await _departmentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DepartmentDropdownDto>> GetForDropdownAsync()
        {
            var departments = await _departmentRepository.GetForDropdownAsync();

            return departments.Select(d => new DepartmentDropdownDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName
            });
        }
    }
}
