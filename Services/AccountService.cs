using AuthMvcApp.Interfaces.Repositories;
using AuthMvcApp.Interfaces.Services;
using AuthMvcApp.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthMvcApp.Services;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<(bool Success, string? Error)> RegisterAsync(RegisterViewModel model)
    {
        var email = model.Email.Trim().ToLowerInvariant();

        if (await _userRepository.ExistsByEmailAsync(email))
            return (false, "An account with this email already exists.");

        var user = new User
        {
            FullName = model.FullName.Trim(),
            Email = email,
            Role = string.IsNullOrWhiteSpace(model.Role) ? "Employee" : model.Role.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return (true, null);
    }

    public async Task<User?> ValidateLoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email.Trim().ToLowerInvariant());
        if (user is null) return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Failed ? null : user;
    }
}
