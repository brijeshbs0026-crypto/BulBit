using AuthMvcApp.Interfaces.Services;
using AuthMvcApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthMvcApp.Controllers;

[Authorize]
public class CalendarController : Controller
{
    private readonly ICalendarService _calendarService;
    private readonly INotificationService _notificationService;

    public CalendarController(
        ICalendarService calendarService,
        INotificationService notificationService)
    {
        _calendarService = calendarService;
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? month, int? year)
    {
        var today = DateTime.Today;

        var currentMonth = month ?? today.Month;
        var currentYear = year ?? today.Year;

        var events = await _calendarService.GetAllAsync();

        ViewBag.CurrentMonth = currentMonth;
        ViewBag.CurrentYear = currentYear;

        return View(events);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CalendarEvent model, int? month, int? year)
    {
        if (string.IsNullOrWhiteSpace(model.Title))
        {
            TempData["ErrorMessage"] = "Event title is required.";
            return RedirectToAction(nameof(Index), new { month, year });
        }

        model.CreatedAt = DateTime.UtcNow;

        await _calendarService.AddAsync(model);

        // Trigger notification for team
        await _notificationService.CreateCalendarEventNotificationAsync(model);

        TempData["SuccessMessage"] = "Event added successfully.";

        return RedirectToAction(nameof(Index), new { month = model.EventDate.Month, year = model.EventDate.Year });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(CalendarEvent model, int? month, int? year)
    {
        if (string.IsNullOrWhiteSpace(model.Title))
        {
            TempData["ErrorMessage"] = "Event title is required.";
            return RedirectToAction(nameof(Index), new { month, year });
        }

        var existing = await _calendarService.GetByIdAsync(model.Id);
        if (existing == null)
        {
            TempData["ErrorMessage"] = "Event not found.";
            return RedirectToAction(nameof(Index), new { month, year });
        }

        existing.Title = model.Title;
        existing.Description = model.Description;
        existing.EventDate = model.EventDate;
        existing.EventType = model.EventType;

        await _calendarService.UpdateAsync(existing);

        TempData["SuccessMessage"] = "Event updated successfully.";

        return RedirectToAction(nameof(Index), new { month = model.EventDate.Month, year = model.EventDate.Year });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, int? month, int? year)
    {
        await _calendarService.DeleteAsync(id);

        TempData["SuccessMessage"] = "Event deleted successfully.";

        return RedirectToAction(nameof(Index), new { month, year });
    }
}