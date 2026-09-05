using AuthMvcApp.Models;

namespace AuthMvcApp.Interfaces.Repositories;

public interface INotificationRepository
{
    Task<List<Notification>> GetUserNotificationsAsync(string? userEmail, string? role, int limit = 20);
    Task<int> GetUnreadCountAsync(string? userEmail, string? role);
    Task<Notification?> GetByIdAsync(int id);
    Task AddAsync(Notification notification);
    Task MarkAsReadAsync(int id);
    Task MarkAllAsReadAsync(string? userEmail, string? role);
    Task DeleteAsync(int id);
}
