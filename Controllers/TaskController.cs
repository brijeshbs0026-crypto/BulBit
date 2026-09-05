using AuthMvcApp.Interfaces.Services;
using AuthMvcApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthMvcApp.Controllers;

[Authorize]
public class TaskController : Controller
{
    private readonly ITaskService _taskService;
    private readonly IEmployeeService _employeeService;
    private readonly INotificationService _notificationService;

    public TaskController(
        ITaskService taskService,
        IEmployeeService employeeService,
        INotificationService notificationService)
    {
        _taskService = taskService;
        _employeeService = employeeService;
        _notificationService = notificationService;
    }

    // =========================
    // TASK LIST
    // =========================
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var tasks = await _taskService.GetAllAsync();

        var employees = await _employeeService.GetAllAsync();

        ViewBag.Employees = employees;

        return View(tasks);
    }

    // =========================
    // CREATE TASK
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(TaskItem model)
    {
        if (string.IsNullOrWhiteSpace(model.Title))
        {
            TempData["ErrorMessage"] = "Task title is required.";
            return RedirectToAction(nameof(Index));
        }

        var employees = await _employeeService.GetAllAsync();

        var employee = employees
            .FirstOrDefault(x => x.Id == model.EmployeeId);

        if (employee == null)
        {
            TempData["ErrorMessage"] = "Please select a valid employee.";
            return RedirectToAction(nameof(Index));
        }

        model.Status = "Pending";
        model.Progress = 0;
        model.CreatedAt = DateTime.UtcNow;

        await _taskService.AddAsync(model);

        // Create notification for the assigned task
        await _notificationService.CreateTaskAssignedNotificationAsync(model, employee);

        TempData["SuccessMessage"] = "Task assigned successfully.";

        return RedirectToAction(nameof(Index));
    }

    // =========================
    // DELETE TASK
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _taskService.DeleteAsync(id);

        TempData["SuccessMessage"] = "Task deleted successfully.";

        return RedirectToAction(nameof(Index));
    }

    // =========================
    // COMPLETE TASK
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(int id)
    {
        var task = await _taskService.GetByIdAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        task.Status = "Completed";
        task.Progress = 100;

        await _taskService.UpdateAsync(task);

        TempData["SuccessMessage"] = "Task marked as completed.";

        return RedirectToAction(nameof(Index));
    }
}