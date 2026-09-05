using AuthMvcApp.Data;
using AuthMvcApp.Interfaces.Repositories;
using AuthMvcApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthMvcApp.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    private IQueryable<Notification> GetUserScopedQuery(string? userEmail, string? role)
    {
        var query = _context.Notifications.AsQueryable();

        if (role == "Admin")
        {
            // Admin sees all notifications or notifications targeted to all/Admin
            return query;
        }

        // Employee sees notifications addressed to their email, or with TargetRole="All" or TargetRole="Employee", or global ones
        return query.Where(n =>
            n.TargetRole == "All" ||
            n.TargetRole == "Employee" ||
            (userEmail != null && n.TargetUserEmail == userEmail) ||
            (n.TargetUserEmail == null && n.TargetRole == null));
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(string? userEmail, string? role, int limit = 20)
    {
        return await GetUserScopedQuery(userEmail, role)
            .OrderByDescending(n => n.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(string? userEmail, string? role)
    {
        return await GetUserScopedQuery(userEmail, role)
            .Where(n => !n.IsRead)
            .CountAsync();
    }

    public async Task<Notification?> GetByIdAsync(int id)
    {
        return await _context.Notifications.FindAsync(id);
    }

    public async Task AddAsync(Notification notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }

    public async Task MarkAsReadAsync(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification != null && !notification.IsRead)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAllAsReadAsync(string? userEmail, string? role)
    {
        var unreadNotifications = await GetUserScopedQuery(userEmail, role)
            .Where(n => !n.IsRead)
            .ToListAsync();

        foreach (var item in unreadNotifications)
        {
            item.IsRead = true;
        }

        if (unreadNotifications.Any())
        {
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}
