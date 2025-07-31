using EmployeeAttendanceTracker.BLL.DTOs;
using EmployeeAttendanceTracker.BLL.Enums;
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
    public class AttendanceService :IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }
        public async Task<CreateAttendanceDto> GetByIdAsync(int id)
        {
            var entity = await _attendanceRepository.GetByIdAsync(id);
            if (entity == null) return null;

            var attendanceWithEmployee = await _attendanceRepository.GetAttendanceWithEmployeeAsync(id);
            if (attendanceWithEmployee == null) return null;

            return new CreateAttendanceDto
            {
                AttendanceId = attendanceWithEmployee.AttendanceId,
                EmployeeId = attendanceWithEmployee.EmployeeId,
                EmployeeName = attendanceWithEmployee.Employee?.FullName,
                DepartmentName = attendanceWithEmployee.Employee?.Department?.DepartmentName,
                EmployeeCode = attendanceWithEmployee.Employee?.EmployeeCode,
                Date = attendanceWithEmployee.Date,
                Status = Enum.Parse<AttendanceStatus>(attendanceWithEmployee.Status)
            };
        }

        public async Task<CreateAttendanceDto> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
        {
            var entity = await _attendanceRepository.GetByEmployeeAndDateAsync(employeeId, date);
            if (entity == null) return null;

            return new CreateAttendanceDto
            {
                AttendanceId = entity.AttendanceId,
                EmployeeId = entity.EmployeeId,
                Date = entity.Date,
                Status = Enum.Parse<AttendanceStatus>(entity.Status)
            };
        }

        public async Task AddAttendanceAsync(CreateAttendanceDto dto)
        {
            if (dto.Date.Date > DateTime.Today)
                throw new Exception("Cannot mark attendance for future dates.");

            bool exists = await _attendanceRepository.ExistsForEmployeeOnDateAsync(dto.EmployeeId, dto.Date);
            if (exists)
                throw new Exception("Attendance already marked for this employee on this date.");

            var attendance = new Attendance
            {
                EmployeeId = dto.EmployeeId,
                Date = dto.Date.Date,
                Status = dto.Status.ToString()
            };

            await _attendanceRepository.AddAsync(attendance);
        }

        public async Task UpdateAttendanceAsync(CreateAttendanceDto dto)
        {
            if (dto.Date.Date > DateTime.Today)
                throw new Exception("Cannot mark attendance for future dates.");

            var existing = await _attendanceRepository.GetByIdAsync(dto.AttendanceId.Value);
            if (existing == null)
                throw new Exception("Record not found.");

            existing.Date = dto.Date.Date;
            existing.Status = dto.Status.ToString();

            await _attendanceRepository.UpdateAsync(existing);
        }

        public async Task UpdateAttendanceStatusAsync(int attendanceId, AttendanceStatus status)
        {
            var existing = await _attendanceRepository.GetByIdAsync(attendanceId);
            if (existing == null)
                throw new Exception("Attendance record not found.");

            if (existing.Date.Date > DateTime.Today)
                throw new Exception("Cannot update attendance for future dates.");

            existing.Status = status.ToString();
            await _attendanceRepository.UpdateAsync(existing);
        }

        public async Task DeleteAttendanceAsync(int id)
        {
            await _attendanceRepository.DeleteAsync(id);
        }

        public async Task<List<CreateAttendanceDto>> FilterAttendancesAsync(int? deptId, int? empId, DateTime? from, DateTime? to)
        {
            var records = await _attendanceRepository.FilterAsync(deptId, empId, from, to);
            return records.Select(a => new CreateAttendanceDto
            {
                AttendanceId = a.AttendanceId,
                EmployeeId = a.EmployeeId,
                EmployeeName = a.Employee.FullName,
                DepartmentName = a.Employee.Department?.DepartmentName,
                EmployeeCode = a.Employee.EmployeeCode,
                Date = a.Date,
                Status = Enum.Parse<AttendanceStatus>(a.Status)
            }).ToList();
        }



    }
}
