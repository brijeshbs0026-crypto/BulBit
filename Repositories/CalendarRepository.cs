using AuthMvcApp.Data;
using AuthMvcApp.Interfaces.Repositories;
using AuthMvcApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthMvcApp.Repositories;

public class CalendarRepository : ICalendarRepository
{
    private readonly ApplicationDbContext _context;

    public CalendarRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CalendarEvent>> GetAllAsync()
    {
        return await _context.CalendarEvents
            .OrderBy(x => x.EventDate)
            .ToListAsync();
    }

    public async Task<CalendarEvent?> GetByIdAsync(int id)
    {
        return await _context.CalendarEvents
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(CalendarEvent calendarEvent)
    {
        _context.CalendarEvents.Add(calendarEvent);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CalendarEvent calendarEvent)
    {
        _context.CalendarEvents.Update(calendarEvent);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.CalendarEvents.FindAsync(id);

        if (item != null)
        {
            _context.CalendarEvents.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}