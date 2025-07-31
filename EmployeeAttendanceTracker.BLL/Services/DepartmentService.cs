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
            ValidateDepartmentName(dto.DepartmentName);

            ValidateDepartmentCode(dto.DepartmentCode);

            ValidateLocation(dto.Location);

            if (!await _departmentRepository.IsNameUniqueAsync(dto.DepartmentName))
                throw new Exception("Department name must be unique.");

            if (!await _departmentRepository.IsCodeUniqueAsync(dto.DepartmentCode))
                throw new Exception("Department code must be unique.");

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
            ValidateDepartmentName(dto.DepartmentName);

            ValidateDepartmentCode(dto.DepartmentCode);

            ValidateLocation(dto.Location);

            var existingDepartment = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (existingDepartment == null)
                throw new Exception("Department not found.");

            if (!await _departmentRepository.IsNameUniqueAsync(dto.DepartmentName, dto.DepartmentId))
                throw new ValidationException("Department name must be unique.");

            if (!await _departmentRepository.IsCodeUniqueAsync(dto.DepartmentCode, dto.DepartmentId))
                throw new ValidationException("Department code must be unique.");

            existingDepartment.DepartmentName = dto.DepartmentName;
            existingDepartment.DepartmentCode = dto.DepartmentCode;
            existingDepartment.Location = dto.Location;

            await _departmentRepository.UpdateAsync(existingDepartment);
            await _departmentRepository.SaveChangesAsync();
        }

        private void ValidateDepartmentName(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                throw new Exception("Department name is required.");

            if (departmentName.Length < 3)
                throw new Exception("Department name must be at least 3 characters long.");

            if (departmentName.Length > 50)
                throw new Exception("Department name cannot exceed 50 characters.");
        }

        private void ValidateDepartmentCode(string departmentCode)
        {
            if (string.IsNullOrWhiteSpace(departmentCode))
                throw new Exception("Department code is required.");

            if (departmentCode.Length != 4)
                throw new Exception("Department code must be exactly 4 characters long.");

            if (!departmentCode.All(char.IsUpper))
                throw new Exception("Department code must be in uppercase.");

            if (!departmentCode.All(char.IsLetter))
                throw new Exception("Department code must contain only alphabetic characters.");
        }

        private void ValidateLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new Exception("Location is required.");

            if (location.Length > 100)
                throw new Exception("Location cannot exceed 100 characters.");
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            await _departmentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<object>> GetDepartmentDropdownAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return departments.Select(d => new {
                departmentId = d.DepartmentId,
                departmentName = d.DepartmentName
            }).ToList();
        }
    }
}
