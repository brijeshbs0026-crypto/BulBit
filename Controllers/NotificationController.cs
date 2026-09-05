using System.Security.Claims;
using AuthMvcApp.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthMvcApp.Controllers;

[Authorize]
public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var role = User.FindFirstValue(ClaimTypes.Role) ?? (User.IsInRole("Admin") ? "Admin" : "Employee");

        var notifications = await _notificationService.GetUserNotificationsAsync(userEmail, role, 20);
        var unreadCount = await _notificationService.GetUnreadCountAsync(userEmail, role);

        var data = notifications.Select(n =>
        {
            var rawUrl = string.IsNullOrWhiteSpace(n.Url) ? "#" : n.Url.Trim();
            if (rawUrl.Equals("/Task", StringComparison.OrdinalIgnoreCase))
                rawUrl = "/Task/Index";
            else if (rawUrl.Equals("/Calendar", StringComparison.OrdinalIgnoreCase))
                rawUrl = "/Calendar/Index";
            else if (rawUrl.Equals("/Employee", StringComparison.OrdinalIgnoreCase))
                rawUrl = "/Employee/Index";
            else if (rawUrl.Equals("/Attendance", StringComparison.OrdinalIgnoreCase))
                rawUrl = "/Attendance/Index";
            else if (rawUrl.Equals("/Client", StringComparison.OrdinalIgnoreCase))
                rawUrl = "/Client/Index";

            return new
            {
                id = n.Id,
                title = n.Title,
                message = n.Message,
                type = n.Type,
                url = rawUrl,
                isRead = n.IsRead,
                createdAt = n.CreatedAt.ToString("o"),
                timeAgo = GetTimeAgo(n.CreatedAt)
            };
        });

        return Json(new
        {
            success = true,
            unreadCount,
            notifications = data
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _notificationService.MarkAsReadAsync(id);
        return Json(new { success = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var role = User.FindFirstValue(ClaimTypes.Role) ?? (User.IsInRole("Admin") ? "Admin" : "Employee");

        await _notificationService.MarkAllAsReadAsync(userEmail, role);
        return Json(new { success = true });
    }

    private static string GetTimeAgo(DateTime dateTimeUtc)
    {
        var span = DateTime.UtcNow - dateTimeUtc;

        if (span.TotalSeconds < 60)
            return "Just now";
        if (span.TotalMinutes < 60)
            return $"{(int)span.TotalMinutes}m ago";
        if (span.TotalHours < 24)
            return $"{(int)span.TotalHours}h ago";
        if (span.TotalDays < 7)
            return $"{(int)span.TotalDays}d ago";

        return dateTimeUtc.ToString("MMM dd");
    }
}
