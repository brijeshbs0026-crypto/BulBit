using AuthMvcApp.Models;

namespace AuthMvcApp.Interfaces.Services;

public interface IAttendanceService
{
    Task<List<Attendance>> GetAllAsync();
    Task<Attendance?> GetTodayAsync(int employeeId);
    Task AddAsync(Attendance attendance);
    Task UpdateAsync(Attendance attendance);
    Task<Attendance?> GetByEmployeeAndDateAsync(int employeeId, DateTime date);

}