using AuthMvcApp.Models;

namespace AuthMvcApp.Interfaces.Services;

public interface INotificationService
{
    Task<List<Notification>> GetUserNotificationsAsync(string? userEmail, string? role, int limit = 20);
    Task<int> GetUnreadCountAsync(string? userEmail, string? role);
    Task MarkAsReadAsync(int id);
    Task MarkAllAsReadAsync(string? userEmail, string? role);
    Task CreateTaskAssignedNotificationAsync(TaskItem task, Employee employee);
    Task CreateCalendarEventNotificationAsync(CalendarEvent calendarEvent);
}
