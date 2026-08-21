using AuthMvcApp.Interfaces.Repositories;
using AuthMvcApp.Interfaces.Services;
using AuthMvcApp.Models;

namespace AuthMvcApp.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepository;

    public AttendanceService(IAttendanceRepository attendanceRepository)
    {
        _attendanceRepository = attendanceRepository;
    }

    public async Task<List<Attendance>> GetAllAsync()
    {
        return await _attendanceRepository.GetAllAsync();
    }

    public async Task AddAsync(Attendance attendance)
    {
        await _attendanceRepository.AddAsync(attendance);
    }

    public async Task UpdateAsync(Attendance attendance)
    {
        await _attendanceRepository.UpdateAsync(attendance);
    }

    public async Task<Attendance?> GetTodayAsync(int employeeId)
    {
        return await _attendanceRepository.GetTodayAsync(employeeId);
    }

    public async Task<Attendance?> GetByEmployeeAndDateAsync(
        int employeeId,
        DateTime date)
    {
        return await _attendanceRepository.GetByEmployeeAndDateAsync(
            employeeId,
            date);
    }



}