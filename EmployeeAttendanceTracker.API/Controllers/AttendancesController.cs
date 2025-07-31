using EmployeeAttendanceTracker.BLL.DTOs;
using EmployeeAttendanceTracker.BLL.Enums;
using EmployeeAttendanceTracker.BLL.ServiceInterfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


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
            var attendances = await _attendanceService.FilterAttendancesAsync(deptId, empId, from, to);
            return View(attendances);
        }

        public async Task<IActionResult> Details(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null)
                return NotFound();

            return View(attendance);
        }

        public async Task<IActionResult> Create()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            ViewData["EmployeeId"] = new SelectList(employees, "EmployeeId", "FullName");
            return View();
        }

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

        public async Task<IActionResult> Edit(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null)
                return NotFound();

            var employees = await _employeeService.GetAllEmployeesAsync();
            ViewData["EmployeeId"] = new SelectList(employees, "EmployeeId", "FullName", attendance.EmployeeId);
            return View(attendance);
        }

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

        public async Task<IActionResult> Delete(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null)
                return NotFound();

            return View(attendance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _attendanceService.DeleteAttendanceAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetStatus(int employeeId, DateTime date)
        {
            var attendance = await _attendanceService.GetByEmployeeAndDateAsync(employeeId, date);
            return Json(new { status = attendance?.Status.ToString() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatusAjax(int attendanceId, string status)
        {
            try
            {
                var attendanceStatus = Enum.Parse<AttendanceStatus>(status);
                await _attendanceService.UpdateAttendanceStatusAsync(attendanceId, attendanceStatus);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

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
                return Json(new { success = false, message = "Invalidd data" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
