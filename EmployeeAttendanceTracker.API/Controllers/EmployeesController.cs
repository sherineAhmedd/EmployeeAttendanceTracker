using EmployeeAttendanceTracker.BLL.DTOs;
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
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public EmployeesController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllEmployeesWithAttendanceAsync();
            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDepartmentsDropdownAsync();
            return View(new CreateEmployeeDto());
        }
        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDepartmentsDropdownAsync(dto.DepartmentId);
                return View(dto);
            }

            try
            {
                await _employeeService.AddEmployeeAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateDepartmentsDropdownAsync(dto.DepartmentId);
                return View(dto);
            }
        }


        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dto = await _employeeService.GetEmployeeDtoByIdAsync(id.Value);
            if (dto == null) return NotFound();

            await PopulateDepartmentsDropdownAsync(dto.DepartmentId);
            return View(dto);
        }



        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDepartmentsDropdownAsync(dto.DepartmentId);
                return View(dto);
            }

            try
            {
                await _employeeService.UpdateEmployeeAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateDepartmentsDropdownAsync(dto.DepartmentId);
                return View(dto);
            }
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _employeeService.GetEmployeeWithDepartmentByIdAsync(id.Value);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Employees/GetForDropdown
        [HttpGet]
        public async Task<IActionResult> GetForDropdown()
        {
            var dropdownData = await _employeeService.GetEmployeeDropdownAsync();
            return Json(dropdownData);
        }


     
        private async Task PopulateDepartmentsDropdownAsync(int? selectedDepartmentId = null)
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "DepartmentName", selectedDepartmentId);
        }

      
    }

    }
