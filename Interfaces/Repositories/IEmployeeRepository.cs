using AuthMvcApp.Models;

namespace AuthMvcApp.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();

        Task<Employee?> GetByIdAsync(int id);

        Task AddAsync(Employee employee);

        Task UpdateAsync(Employee employee);

        Task DeleteAsync(int id);
        Task<Employee?> GetByEmailAsync(string email);

       
    }
}