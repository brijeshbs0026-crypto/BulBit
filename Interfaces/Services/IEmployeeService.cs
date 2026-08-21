using AuthMvcApp.Models;

namespace AuthMvcApp.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync();

        Task<Employee?> GetByIdAsync(int id);

        Task AddAsync(Employee employee);

        Task UpdateAsync(Employee employee);

        Task DeleteAsync(int id);

        Task<Employee?> GetByEmailAsync(string email);
    }
}