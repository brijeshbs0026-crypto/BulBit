using AuthMvcApp.Models;

namespace AuthMvcApp.Interfaces.Services;

public interface IAccountService
{
    Task<(bool Success, string? Error)> RegisterAsync(RegisterViewModel model);
    Task<User?> ValidateLoginAsync(string email, string password);
}
