using AuthMvcApp.Interfaces.Repositories;
using AuthMvcApp.Interfaces.Services;
using AuthMvcApp.Models;

namespace AuthMvcApp.Services;

public class CalendarService : ICalendarService
{
    private readonly ICalendarRepository _repository;

    public CalendarService(ICalendarRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CalendarEvent>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<CalendarEvent?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(CalendarEvent calendarEvent)
    {
        await _repository.AddAsync(calendarEvent);
    }

    public async Task UpdateAsync(CalendarEvent calendarEvent)
    {
        await _repository.UpdateAsync(calendarEvent);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}