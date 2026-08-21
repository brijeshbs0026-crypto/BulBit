using AuthMvcApp.Interfaces.Services;
using AuthMvcApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthMvcApp.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly ITaskService _taskService;
    private readonly IAttendanceService _attendanceService;

    public DashboardController(
        IEmployeeService employeeService,
        ITaskService taskService,
        IAttendanceService attendanceService)
    {
        _employeeService = employeeService;
        _taskService = taskService;
        _attendanceService = attendanceService;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _employeeService.GetAllAsync();
        var tasks = await _taskService.GetAllAsync();
        var attendance = await _attendanceService.GetAllAsync();

        var today = DateTime.Today;

        var todayAttendance = attendance
            .Where(x => x.AttendanceDate.Date == today)
            .ToList();

        var presentToday = todayAttendance.Count(x =>
            x.Status == "Present" || x.Status == "Late");

        var totalEmployees = employees.Count;

        var attendancePercentage = totalEmployees == 0
            ? 0
            : Math.Round((double)presentToday / totalEmployees * 100, 1);

        var model = new DashboardViewModel
        {
            UserName = User.FindFirstValue(ClaimTypes.Name) ?? "User",

            TotalEmployees = totalEmployees,

            ActiveTasks = tasks.Count(x =>
                x.Status != "Completed"),

            PresentToday = presentToday,

            AttendancePercentage = attendancePercentage,

            Employees = employees
                .Take(4)
                .ToList(),

            TotalTasks = tasks.Count,

            CompletedTasks = tasks.Count(x =>
                x.Status == "Completed"),

            InProgressTasks = tasks.Count(x =>
                x.Status == "In Progress")
        };

        return View(model);
    }
}