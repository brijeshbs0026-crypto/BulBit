using AuthMvcApp.Interfaces.Services;
using AuthMvcApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthMvcApp.Controllers;

[Authorize]
public class CalendarController : Controller
{
    private readonly ICalendarService _calendarService;

    public CalendarController(ICalendarService calendarService)
    {
        _calendarService = calendarService;
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
    public async Task<IActionResult> Create(CalendarEvent model)
    {
        if (string.IsNullOrWhiteSpace(model.Title))
        {
            TempData["ErrorMessage"] = "Event title is required.";
            return RedirectToAction(nameof(Index));
        }

        model.CreatedAt = DateTime.UtcNow;

        await _calendarService.AddAsync(model);

        TempData["SuccessMessage"] = "Event added successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _calendarService.DeleteAsync(id);

        TempData["SuccessMessage"] = "Event deleted successfully.";

        return RedirectToAction(nameof(Index));
    }
}