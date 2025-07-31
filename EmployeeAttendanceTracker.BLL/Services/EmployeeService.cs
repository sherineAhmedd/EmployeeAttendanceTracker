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
            
            ValidateFullName(dto.FullName);

            ValidateEmailFormat(dto.Email);

            var isUnique = await _employeeRepository.IsEmailUniqueAsync(dto.Email);
            if (!isUnique)
                throw new Exception("Email already exists.");

            var employeeCode = await _employeeRepository.GenerateUniqueEmployeeCodeAsync();

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
          
            ValidateFullName(dto.FullName);

            ValidateEmailFormat(dto.Email);

            var existing = await _employeeRepository.GetByIdAsync(dto.EmployeeId); 
            if (existing == null)
                throw new Exception("Employee not found.");

            var isUnique = await _employeeRepository.IsEmailUniqueAsync(dto.Email, existing.EmployeeId);
            if (!isUnique)
                throw new Exception("Email already exists.");
            existing.FullName = dto.FullName;
            existing.Email = dto.Email;
            existing.DepartmentId = dto.DepartmentId;

            await _employeeRepository.UpdateAsync(existing);
        }
        public async Task<EmployeeDto?> GetEmployeeDtoByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return null;

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
        public async Task<IEnumerable<object>> GetEmployeeDropdownAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return employees.Select(e => new {
                employeeId = e.EmployeeId,
                fullName = e.FullName
            }).ToList();
        }
       

        public async Task<bool> EmployeeExistsAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee != null;
        }

        private void ValidateFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new Exception("Full name is required.");

            var names = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (names.Length != 4)
                throw new Exception("Full name must contain exactly four names.");

            foreach (var name in names)
            {
                if (name.Length < 2)
                    throw new Exception("Each name must be at least 2 characters long.");

                if (!name.All(char.IsLetter))
                    throw new Exception("Names can only contain letters.");
            }
        }

        private void ValidateEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email is required.");

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                    throw new Exception("Invalid email format.");
            }
            catch
            {
                throw new Exception("Invalid email format.");
            }
        }


    }
}

