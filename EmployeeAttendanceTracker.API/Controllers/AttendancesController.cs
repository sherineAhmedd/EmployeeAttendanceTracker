using EmployeeAttendanceTracker.BLL.DTOs;
using EmployeeAttendanceTracker.BLL.Enums;
using EmployeeAttendanceTracker.BLL.ServiceInterfaces;
using EmployeeAttendanceTracker.DAL.Context;
using EmployeeAttendanceTracker.DAL.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAttendanceTracker.API.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IEmployeeService _employeeService;

        public AttendancesController(IAttendanceService attendanceService, IEmployeeService employeeService)
        {
            _attendanceService = attendanceService;
            _employeeService = employeeService;
        }
        // GET: Attendances
        public async Task<IActionResult> Index(int? deptId, int? empId, DateTime? from, DateTime? to, int page = 1)
        {
            // Debug: Log the filter parameters
            System.Diagnostics.Debug.WriteLine($"Filter params: deptId={deptId}, empId={empId}, from={from}, to={to}");
            
            var pageSize = 10; // Items per page
            var attendances = await _attendanceService.FilterAttendancesAsync(deptId, empId, from, to);
            
            // Calculate pagination
            var totalItems = attendances.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var skip = (page - 1) * pageSize;
            var pagedAttendances = attendances.Skip(skip).Take(pageSize).ToList();
            
            // Set ViewBag for pagination
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.HasNextPage = page < totalPages;
            
            return View(pagedAttendances);
        }

        // GET: Attendances/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null)
                return NotFound();

            return View(attendance);
        }

        // GET: Attendances/Create
        public async Task<IActionResult> Create()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            ViewData["EmployeeId"] = new SelectList(employees, "EmployeeId", "FullName");
            return View();
        }

        // POST: Attendances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAttendanceDto dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _attendanceService.AddAttendanceAsync(dto);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            var employees = await _employeeService.GetAllEmployeesAsync();
            ViewData["EmployeeId"] = new SelectList(employees, "EmployeeId", "FullName", dto.EmployeeId);
            return View(dto);
        }

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null)
                return NotFound();

            var employees = await _employeeService.GetAllEmployeesAsync();
            ViewData["EmployeeId"] = new SelectList(employees, "EmployeeId", "FullName", attendance.EmployeeId);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateAttendanceDto dto)
        {
            if (id != dto.AttendanceId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _attendanceService.UpdateAttendanceAsync(dto);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            var employees = await _employeeService.GetAllEmployeesAsync();
            ViewData["EmployeeId"] = new SelectList(employees, "EmployeeId", "FullName", dto.EmployeeId);
            return View(dto);
        }

        // GET: Attendances/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null)
                return NotFound();

            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _attendanceService.DeleteAttendanceAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendances/GetStatus
        [HttpGet]
        public async Task<IActionResult> GetStatus(int employeeId, DateTime date)
        {
            var attendance = await _attendanceService.GetByEmployeeAndDateAsync(employeeId, date);
            return Json(new { status = attendance?.Status.ToString() });
        }

        // POST: Attendances/UpdateStatusAjax (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatusAjax(int attendanceId, string status)
        {
            try
            {
                var attendance = await _attendanceService.GetByIdAsync(attendanceId);
                if (attendance == null)
                    return Json(new { success = false, message = "Attendance record not found" });

                // Update the status
                attendance.Status = Enum.Parse<AttendanceStatus>(status);
                await _attendanceService.UpdateAttendanceAsync(attendance);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Attendances/CreateAjax (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax([FromBody] CreateAttendanceDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _attendanceService.AddAttendanceAsync(dto);
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Invalid data" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
