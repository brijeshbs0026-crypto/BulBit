using AuthMvcApp.Models;

namespace AuthMvcApp.Interfaces.Repositories;

public interface ICalendarRepository
{
    Task<List<CalendarEvent>> GetAllAsync();
    Task<CalendarEvent?> GetByIdAsync(int id);
    Task AddAsync(CalendarEvent calendarEvent);
    Task UpdateAsync(CalendarEvent calendarEvent);
    Task DeleteAsync(int id);
}