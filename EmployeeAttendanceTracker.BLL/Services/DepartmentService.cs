using EmployeeAttendanceTracker.BLL.DTOs;
using EmployeeAttendanceTracker.BLL.ServiceInterfaces;
using EmployeeAttendanceTracker.DAL.Data.Entities;
using EmployeeAttendanceTracker.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("Invalid department ID.");

            var department = await _departmentRepository.GetByIdAsync(id);

            if (department == null)
                return null;

            return new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                DepartmentCode = department.DepartmentCode,
                Location = department.Location
            };
        }

        public async Task AddDepartmentAsync(CreateDepartmentDto dto)
        {
            // Use dto for validation
            if (!await _departmentRepository.IsNameUniqueAsync(dto.DepartmentName))
                throw new Exception("Department name must be unique.");

            if (!await _departmentRepository.IsCodeUniqueAsync(dto.DepartmentCode))
                throw new Exception("Department code must be unique.");

            // Map dto to entity
            var department = new Department
            {
                DepartmentName = dto.DepartmentName,
                DepartmentCode = dto.DepartmentCode,
                Location = dto.Location
            };

            await _departmentRepository.AddAsync(department);
            await _departmentRepository.SaveChangesAsync();
        }

        public async Task<DepartmentDto?> GetDepartmentDtoByIdAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null) return null;

            return new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                DepartmentCode = department.DepartmentCode,
                Location = department.Location
            };
        }


        public async Task UpdateDepartmentAsync(DepartmentDto dto)
        {
            var existingDepartment = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (existingDepartment == null)
                throw new Exception("Department not found.");

            // Business validation
            if (!await _departmentRepository.IsNameUniqueAsync(dto.DepartmentName, dto.DepartmentId))
                throw new ValidationException("Department name must be unique.");

            if (!await _departmentRepository.IsCodeUniqueAsync(dto.DepartmentCode, dto.DepartmentId))
                throw new ValidationException("Department code must be unique.");

            // Update fields
            existingDepartment.DepartmentName = dto.DepartmentName;
            existingDepartment.DepartmentCode = dto.DepartmentCode;
            existingDepartment.Location = dto.Location;

            await _departmentRepository.UpdateAsync(existingDepartment);
            await _departmentRepository.SaveChangesAsync();
        }


        public async Task DeleteDepartmentAsync(int id)
        {
            await _departmentRepository.DeleteAsync(id);
        }

        //public async Task<IEnumerable<DepartmentDropdownDto>> GetForDropdownAsync()
        //{
        //    var departments = await _departmentRepository.GetForDropdownAsync();

        //    return departments.Select(d => new DepartmentDropdownDto
        //    {
        //        DepartmentId = d.DepartmentId,
        //        DepartmentName = d.DepartmentName
        //    });
        //}
    }
}
