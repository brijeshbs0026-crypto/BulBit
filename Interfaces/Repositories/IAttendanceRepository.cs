using AuthMvcApp.Models;

namespace AuthMvcApp.Interfaces.Repositories;

public interface IAttendanceRepository
{
    Task<List<Attendance>> GetAllAsync();
    Task<Attendance?> GetByIdAsync(int id);
    Task<Attendance?> GetTodayAsync(int employeeId);
    Task AddAsync(Attendance attendance);
    Task UpdateAsync(Attendance attendance);

    Task<Attendance?> GetByEmployeeAndDateAsync(
    int employeeId,
    DateTime date);



  
}