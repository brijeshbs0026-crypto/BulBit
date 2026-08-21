using AuthMvcApp.Interfaces.Services;
using AuthMvcApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthMvcApp.Controllers;

[Authorize]
public class AttendanceController : Controller
{
    private readonly IAttendanceService _attendanceService;
    private readonly IEmployeeService _employeeService;

    public AttendanceController(
        IAttendanceService attendanceService,
        IEmployeeService employeeService)
    {
        _attendanceService = attendanceService;
        _employeeService = employeeService;
    }

    //[HttpGet]
    //public async Task<IActionResult> Index()
    //{
    //    var attendance = await _attendanceService.GetAllAsync();

    //    var email = User.Identity?.Name;

    //    var employees = await _employeeService.GetAllAsync();

    //    var currentEmployee = employees.FirstOrDefault(x =>
    //        x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    //    ViewBag.CurrentEmployee = currentEmployee;

    //    return View(attendance);
    //}


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var currentEmployee = await _employeeService.GetByEmailAsync(email);

        if (currentEmployee == null)
        {
            TempData["ErrorMessage"] = "Employee profile not found.";
            return View(new List<Attendance>());
        }

        // Get ALL attendance records
        var attendance = await _attendanceService.GetAllAsync();

        // Send logged-in employee to View
        ViewBag.CurrentEmployee = currentEmployee;

        return View(attendance);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
  
    public async Task<IActionResult> CheckIn()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var employees = await _employeeService.GetAllAsync();

        var employee = employees.FirstOrDefault(x =>
            x.Email != null &&
            x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (employee == null)
        {
            TempData["ErrorMessage"] = "Employee profile not found.";
            return RedirectToAction(nameof(Index));
        }

        var today = DateTime.Today;

        // Check if employee already checked in today
        var existing = await _attendanceService.GetByEmployeeAndDateAsync(
            employee.Id,
            today);

        if (existing != null)
        {
            TempData["ErrorMessage"] = "You have already checked in today.";
            return RedirectToAction(nameof(Index));
        }

        // Current check-in time
        var checkInTime = DateTime.Now;

        // 9:15 AM is the grace period
        var lateTime = new TimeSpan(9, 15, 0);

        var status = checkInTime.TimeOfDay > lateTime
            ? "Late"
            : "Present";

        var attendance = new Attendance
        {
            EmployeeId = employee.Id,
            AttendanceDate = today,
            CheckIn = checkInTime,
            Status = status,
            Hours = 0
        };

        await _attendanceService.AddAsync(attendance);

        if (status == "Late")
        {
            TempData["SuccessMessage"] =
                $"Check-in successful. You are marked Late ({checkInTime:hh:mm tt}).";
        }
        else
        {
            TempData["SuccessMessage"] =
                $"Check-in successful ({checkInTime:hh:mm tt}).";
        }

        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckOut()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var employee = await _employeeService.GetByEmailAsync(email);

        if (employee == null)
        {
            TempData["ErrorMessage"] = "Employee profile not found.";
            return RedirectToAction(nameof(Index));
        }

        var attendance = await _attendanceService.GetTodayAsync(employee.Id);

        if (attendance == null)
        {
            TempData["ErrorMessage"] = "You have not checked in today.";
            return RedirectToAction(nameof(Index));
        }

        if (attendance.CheckOut != null)
        {
            TempData["ErrorMessage"] = "You have already checked out.";
            return RedirectToAction(nameof(Index));
        }

        attendance.CheckOut = DateTime.Now;

        if (attendance.CheckIn.HasValue)
        {
            var duration = attendance.CheckOut.Value - attendance.CheckIn.Value;
            attendance.Hours = Math.Round(duration.TotalHours, 2);
        }

        await _attendanceService.UpdateAsync(attendance);

        TempData["SuccessMessage"] = "Checked out successfully.";

        return RedirectToAction(nameof(Index));
    }


}