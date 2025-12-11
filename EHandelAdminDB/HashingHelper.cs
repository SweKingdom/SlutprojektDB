using System.Security.Cryptography;

namespace EHandelAdminDB;

public class HashingHelper
{
    public static string GenerateSalt(int size = 16)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(size);
        return Convert.ToBase64String(saltBytes);
    }

    public static string HashWithSalt(string value, string base64Salt, int iterations = 100_000, int hashLenght = 32)
    {
        var saltBytes = Convert.FromBase64String(base64Salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password: value,
            salt: saltBytes,
            iterations: iterations,
            hashAlgorithm: HashAlgorithmName.SHA256);
        
        var hashBytes = pbkdf2.GetBytes(hashLenght);
        return Convert.ToBase64String(hashBytes);
    }

    public static bool Verify(string value, string base64Salt, string exoectedBase64Hash)
    {
        var computedHash = HashWithSalt(value, base64Salt);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(exoectedBase64Hash),
            Convert.FromBase64String(computedHash));
    }
}