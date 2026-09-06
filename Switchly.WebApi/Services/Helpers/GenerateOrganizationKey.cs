using System.Security.Cryptography;

namespace Switchly.WebApi.Services.Helpers;

public static class GenerateOrganizationKey
{
    public static string GenerateKey()
    {
        var bytes = RandomNumberGenerator.GetBytes(16); // 128-bit
        var token = Convert.ToHexString(bytes);         // 32 char hex
        return $"pub_{token}";
    }
}