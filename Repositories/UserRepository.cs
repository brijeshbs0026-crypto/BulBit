using AuthMvcApp.Data;
using AuthMvcApp.Interfaces.Repositories;
using AuthMvcApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthMvcApp.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) => _context = context;

    public Task<User?> GetByEmailAsync(string email) =>
        _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

    public Task<bool> ExistsByEmailAsync(string email) =>
        _context.Users.AnyAsync(x => x.Email == email);

    public Task AddAsync(User user) => _context.Users.AddAsync(user).AsTask();

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
