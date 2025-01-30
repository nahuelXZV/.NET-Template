using Microsoft.AspNetCore.Identity;

namespace Application.Helpers;

public class PasswordHasherHelper
{
    public static string HashPassword(string email, string password)
    {
        PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();
        var hashedPassword = _passwordHasher.HashPassword(email, password);
        return hashedPassword;
    }

    public static bool VerifyPassword(string email, string hashedPassword, string providedPassword)
    {
        PasswordHasher<string> _passwordHasher = new();
        var result = _passwordHasher.VerifyHashedPassword(email, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
