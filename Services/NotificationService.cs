using AuthMvcApp.Interfaces.Repositories;
using AuthMvcApp.Interfaces.Services;
using AuthMvcApp.Models;

namespace AuthMvcApp.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(string? userEmail, string? role, int limit = 20)
    {
        return await _notificationRepository.GetUserNotificationsAsync(userEmail, role, limit);
    }

    public async Task<int> GetUnreadCountAsync(string? userEmail, string? role)
    {
        return await _notificationRepository.GetUnreadCountAsync(userEmail, role);
    }

    public async Task MarkAsReadAsync(int id)
    {
        await _notificationRepository.MarkAsReadAsync(id);
    }

    public async Task MarkAllAsReadAsync(string? userEmail, string? role)
    {
        await _notificationRepository.MarkAllAsReadAsync(userEmail, role);
    }

    public async Task CreateTaskAssignedNotificationAsync(TaskItem task, Employee employee)
    {
        var notification = new Notification
        {
            Title = "New Task Assigned",
            Message = $"Task \"{task.Title}\" was assigned to {employee.FullName} (Priority: {task.Priority}).",
            Type = "Task",
            TargetUserEmail = employee.Email,
            TargetEmployeeId = employee.Id,
            TargetRole = "All", // Visible to Admin as audit log and to Employee as assigned task
            Url = "/Task/Index",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
    }

    public async Task CreateCalendarEventNotificationAsync(CalendarEvent calendarEvent)
    {
        var eventDateStr = calendarEvent.EventDate.ToString("MMM dd, yyyy");
        var notification = new Notification
        {
            Title = "New Calendar Event",
            Message = $"New event \"{calendarEvent.Title}\" scheduled on {eventDateStr}.",
            Type = "Event",
            TargetRole = "All",
            Url = "/Calendar/Index",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
    }
}
