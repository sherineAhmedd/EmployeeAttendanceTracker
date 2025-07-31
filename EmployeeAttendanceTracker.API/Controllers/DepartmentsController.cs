using EmployeeAttendanceTracker.BLL.DTOs;
using EmployeeAttendanceTracker.BLL.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

        public async Task<IActionResult> Details(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            return department == null ? View("NotFound") : View(department);
        }


        public IActionResult Create()
        {
            return View(new CreateDepartmentDto());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                // Pass the DTO directly; the service handles mapping, validation, and savingg
                await _departmentService.AddDepartmentAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dto = await _departmentService.GetDepartmentDtoByIdAsync(id.Value);
            if (dto == null) return NotFound();

            return View(dto);
        }



        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentDto dto)
        {
            if (id != dto.DepartmentId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _departmentService.UpdateDepartmentAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
            }

            return View(dto);
        }



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

        // GET: Departments/GetForDropdown
        [HttpGet]
        public async Task<IActionResult> GetForDropdown()
        {
            var dropdownData = await _departmentService.GetDepartmentDropdownAsync();
            return Json(dropdownData);
        }
    }

}
