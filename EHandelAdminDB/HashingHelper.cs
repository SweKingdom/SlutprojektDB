using System.Security.Cryptography;

namespace EHandelAdminDB;

/// <summary>
/// Provides secure hashing utilities including salt generation
/// </summary>
public class HashingHelper
{
    ///Generates a cryptographically secure random salt
    public static string GenerateSalt(int size = 16)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(size);
        return Convert.ToBase64String(saltBytes);
    }

    // <summary>
    /// Hashes a value together with a salt using PBKDF2 (SHA256).
    /// </summary>
    /// <param name="value">The plaintext value to hash.</param>
    /// <param name="base64Salt">Base64 encoded salt string.</param>
    /// <param name="iterations">Number of hashing iterations (default: 100,000).</param>
    /// <param name="hashLength">Length of the generated hash in bytes (default: 32).</param>
    /// <returns>Base64 encoded hash string.</returns>
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

    /// <summary>
    /// Verifies a value by hashing it again and comparing hashes in fixed time.
    /// </summary>
    /// <param name="value">The plaintext value to verify.</param>
    /// <param name="base64Salt">Base64 encoded salt used during hashing.</param>
    /// <param name="expectedBase64Hash">Previously stored Base64 encoded hash.</param>
    /// <returns>True if the hashes match; otherwise false.</returns>
    public static bool Verify(string value, string base64Salt, string exoectedBase64Hash)
    {
        var computedHash = HashWithSalt(value, base64Salt);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(exoectedBase64Hash),
            Convert.FromBase64String(computedHash));
    }
}