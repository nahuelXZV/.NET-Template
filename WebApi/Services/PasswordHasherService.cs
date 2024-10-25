using Microsoft.AspNetCore.Identity;

namespace WebApi.Services;

public class PasswordHasherService
{
    public static string HashPassword(string password)
    {
        PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();
        var hashedPassword = _passwordHasher.HashPassword(null, password);
        return hashedPassword;
    }

    public static bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();
        var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
