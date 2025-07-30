using EmployeeAttendanceTracker.BLL.ServiceInterfaces;
using EmployeeAttendanceTracker.DAL.Data.Entities;
using EmployeeAttendanceTracker.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeAttendanceTracker.BLL.DTOs;


namespace EmployeeAttendanceTracker.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAttendanceRepository _attendanceRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IAttendanceRepository attendanceRepository)
        {
            _employeeRepository = employeeRepository;
            _attendanceRepository = attendanceRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesWithAttendanceAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var employeeDtos = new List<EmployeeDto>();
            
            foreach (var employee in employees)
            {
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;
                var attendanceSummary = await CalculateAttendanceSummaryAsync(employee.EmployeeId, currentMonth, currentYear);
                
                employeeDtos.Add(new EmployeeDto
                {
                    EmployeeId = employee.EmployeeId,
                    FullName = employee.FullName,
                    Email = employee.Email,
                    DepartmentId = employee.DepartmentId,
                    EmployeeCode = employee.EmployeeCode,
                    DepartmentName = employee.Department?.DepartmentName,
                    PresentCount = attendanceSummary.PresentCount,
                    AbsentCount = attendanceSummary.AbsentCount,
                    AttendancePercentage = attendanceSummary.AttendancePercentage
                });
            }
            
            return employeeDtos;
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task AddEmployeeAsync(CreateEmployeeDto dto)
        {
            // Validate Email Uniqueness
            var isUnique = await _employeeRepository.IsEmailUniqueAsync(dto.Email);
            if (!isUnique)
                throw new Exception("Email already exists.");

            // Generate Employee Code
            var employeeCode = await _employeeRepository.GenerateUniqueEmployeeCodeAsync();

            // Map DTO to Entity
            var employee = new Employee
            {
                FullName = dto.FullName,
                Email = dto.Email,
                DepartmentId = dto.DepartmentId,
                EmployeeCode = employeeCode
            };

            await _employeeRepository.AddAsync(employee);
        }


        public async Task UpdateEmployeeAsync(EmployeeDto dto)
        {
           

            var existing = await _employeeRepository.GetByIdAsync(dto.EmployeeId); // Use .Value to convert nullable int to int
            if (existing == null)
                throw new Exception("Employee not found.");

            // Validate email uniqueness
            var isUnique = await _employeeRepository.IsEmailUniqueAsync(dto.Email, existing.EmployeeId);
            if (!isUnique)
                throw new Exception("Email already exists.");

            // Update fields
            existing.FullName = dto.FullName;
            existing.Email = dto.Email;
            existing.DepartmentId = dto.DepartmentId;

            await _employeeRepository.UpdateAsync(existing);
        }
        public async Task<EmployeeDto?> GetEmployeeDtoByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return null;

            // Calculate attendance summary for current month
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var attendanceSummary = await CalculateAttendanceSummaryAsync(employee.EmployeeId, currentMonth, currentYear);

            return new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                EmployeeCode = employee.EmployeeCode,
                DepartmentName = employee.Department?.DepartmentName,
                PresentCount = attendanceSummary.PresentCount,
                AbsentCount = attendanceSummary.AbsentCount,
                AttendancePercentage = attendanceSummary.AttendancePercentage
            };
        }

        private async Task<(int PresentCount, int AbsentCount, double AttendancePercentage)> CalculateAttendanceSummaryAsync(int employeeId, int month, int year)
        {
            // Get all attendance records for the employee in the specified month and year
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            
            var attendances = await _attendanceRepository.FilterAsync(null, employeeId, startDate, endDate);
            
            var presentCount = attendances.Count(a => a.Status == "Present");
            var absentCount = attendances.Count(a => a.Status == "Absent");
            var totalDays = presentCount + absentCount;
            
            double percentage = totalDays > 0 ? (double)presentCount / totalDays * 100 : 0;
            
            return (presentCount, absentCount, Math.Round(percentage, 2));
        }


        public async Task DeleteEmployeeAsync(int id)
        {
            await _employeeRepository.DeleteAsync(id);
        }
        public async Task<Employee> GetEmployeeWithDepartmentByIdAsync(int id)
        {
           return  await _employeeRepository.GetByIdAsync(id);

        }
       

        public async Task<bool> EmployeeExistsAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee != null;
        }


    }
}

