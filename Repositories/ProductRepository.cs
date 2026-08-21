using AuthMvcApp.Data;
using AuthMvcApp.Interfaces.Repositories;
using AuthMvcApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthMvcApp.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) => _context = context;

    public Task<List<Product>> GetAllAsync() =>
        _context.Products.AsNoTracking().OrderByDescending(x => x.Id).ToListAsync();

    public Task<Product?> GetByIdAsync(int id) =>
        _context.Products.FirstOrDefaultAsync(x => x.Id == id);

    public Task AddAsync(Product product) => _context.Products.AddAsync(product).AsTask();

    public void Update(Product product) => _context.Products.Update(product);

    public void Delete(Product product) => _context.Products.Remove(product);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
