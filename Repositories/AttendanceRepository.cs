using AuthMvcApp.Data;
using AuthMvcApp.Interfaces.Repositories;
using AuthMvcApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthMvcApp.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly ApplicationDbContext _context;

    public AttendanceRepository(ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<List<Attendance>> GetAllAsync()
    {
        return await _context.Attendances
            .Include(x => x.Employee)
            .OrderByDescending(x => x.AttendanceDate)
            .ToListAsync();
    }

    public async Task<Attendance?> GetByIdAsync(int id)
    {
        return await _context.Attendances
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Attendance?> GetTodayAsync(int employeeId)
    {
        var today = DateTime.Today;

        return await _context.Attendances
            .FirstOrDefaultAsync(x =>
                x.EmployeeId == employeeId &&
                x.AttendanceDate.Date == today);
    }

    public async Task AddAsync(Attendance attendance)
    {
        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Attendance attendance)
    {
        _context.Attendances.Update(attendance);
        await _context.SaveChangesAsync();
    }
    public async Task<Attendance?> GetByEmployeeAndDateAsync(
    int employeeId,
    DateTime date)
    {
        return await _context.Attendances
            .FirstOrDefaultAsync(x =>
                x.EmployeeId == employeeId &&
                x.AttendanceDate.Date == date.Date);
    }






}