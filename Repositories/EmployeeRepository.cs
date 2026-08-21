using AuthMvcApp.Data;
using AuthMvcApp.Interfaces.Repositories;
using AuthMvcApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthMvcApp.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                // Remove related Attendance records
                var attendances = await _context.Attendances
                    .Where(x => x.EmployeeId == id)
                    .ToListAsync();

                if (attendances.Any())
                {
                    _context.Attendances.RemoveRange(attendances);
                }

                // Remove related Task records
                var tasks = await _context.Tasks
                    .Where(x => x.EmployeeId == id)
                    .ToListAsync();

                if (tasks.Any())
                {
                    _context.Tasks.RemoveRange(tasks);
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }



        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}