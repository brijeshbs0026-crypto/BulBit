using AuthMvcApp.Models;

namespace AuthMvcApp.Interfaces.Services;

public interface ICalendarService
{
    Task<List<CalendarEvent>> GetAllAsync();
    Task<CalendarEvent?> GetByIdAsync(int id);
    Task AddAsync(CalendarEvent calendarEvent);
    Task UpdateAsync(CalendarEvent calendarEvent);
    Task DeleteAsync(int id);
}