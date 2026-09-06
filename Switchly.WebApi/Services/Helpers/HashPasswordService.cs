using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using Switchly.WebApi.Entities;

namespace Switchly.WebApi.Services.Helpers;

public static class HashPasswordService
{
    private static readonly PasswordHasher<User> PasswordHasher = new();

    public static string Hash(User user, string password)
        => PasswordHasher.HashPassword(user, password);

    public static PasswordVerificationResult Verify(User user, string password)
    {
        var verificationResult = PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (verificationResult != PasswordVerificationResult.Failed)
            return verificationResult;

        // Existing SHA-256 hashes are accepted once so they can be upgraded on successful sign-in.
        return VerifyLegacySha256(user.PasswordHash, password)
            ? PasswordVerificationResult.SuccessRehashNeeded
            : PasswordVerificationResult.Failed;
    }

    private static bool VerifyLegacySha256(string storedHash, string password)
    {
        try
        {
            var storedBytes = Convert.FromBase64String(storedHash);
            var passwordBytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(password));

            return storedBytes.Length == passwordBytes.Length &&
                   CryptographicOperations.FixedTimeEquals(storedBytes, passwordBytes);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
