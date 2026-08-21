using AuthMvcApp.Interfaces.Repositories;
using AuthMvcApp.Interfaces.Services;
using AuthMvcApp.Models;

namespace AuthMvcApp.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository) => _repository = repository;

    public Task<List<Product>> GetAllAsync() => _repository.GetAllAsync();

    public Task<Product?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

    public async Task CreateAsync(ProductViewModel model)
    {
        var product = new Product
        {
            Name = model.Name.Trim(),
            Description = model.Description?.Trim(),
            Price = model.Price,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(ProductViewModel model)
    {
        var product = await _repository.GetByIdAsync(model.Id);
        if (product is null) return false;

        product.Name = model.Name.Trim();
        product.Description = model.Description?.Trim();
        product.Price = model.Price;

        _repository.Update(product);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null) return false;

        _repository.Delete(product);
        await _repository.SaveChangesAsync();
        return true;
    }
}
