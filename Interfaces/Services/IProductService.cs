using AuthMvcApp.Models;

namespace AuthMvcApp.Interfaces.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task CreateAsync(ProductViewModel model);
    Task<bool> UpdateAsync(ProductViewModel model);
    Task<bool> DeleteAsync(int id);
}
