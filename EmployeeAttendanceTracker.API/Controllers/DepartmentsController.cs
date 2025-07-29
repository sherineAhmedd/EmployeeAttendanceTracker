using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeAttendanceTracker.DAL.Context;
using EmployeeAttendanceTracker.DAL.Data.Entities;
using EmployeeAttendanceTracker.BLL.ServiceInterfaces;
using EmployeeAttendanceTracker.BLL.Services;
using EmployeeAttendanceTracker.BLL.DTOs;

namespace EmployeeAttendanceTracker.API.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return View(departments);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department == null) return NotFound();

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View(new CreateDepartmentDto());
        }

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentDto dto)
        {
            if (ModelState.IsValid)
            {
                var department = new Department
                {
                    DepartmentName = dto.DepartmentName,
                    DepartmentCode = dto.DepartmentCode,
                    Location = dto.Location
                };

                await _departmentService.AddDepartmentAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department == null) return NotFound();

            var dto = new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                DepartmentCode = department.DepartmentCode,
                Location = department.Location
            };

            return View(dto);
        }


        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentDto dto)
        {
            if (id != dto.DepartmentId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var existingDepartment = await _departmentService.GetDepartmentByIdAsync(id);
                if (existingDepartment == null)
                    return NotFound();

                // Update only the properties you allow to change
                existingDepartment.DepartmentName = dto.DepartmentName;
                existingDepartment.DepartmentCode = dto.DepartmentCode;
                existingDepartment.Location = dto.Location;

                try
                {
                    await _departmentService.UpdateDepartmentAsync(existingDepartment);
                }
                catch (Exception)
                {
                    // Optionally check again to see if the record was deleted
                    if (await _departmentService.GetDepartmentByIdAsync(dto.DepartmentId) == null)
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(dto);
        }


        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department == null) return NotFound();

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _departmentService.DeleteDepartmentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
