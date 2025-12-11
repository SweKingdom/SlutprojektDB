namespace EHandelAdminDB;


/// <summary>
/// Simple symmetric encryption helper using XOR + Base64 encoding
/// </summary>
public class EncryptionHelper
{
    // A very basic (and insecure) symmetric key used for XOR encryption
    private const byte Key = 0x42; // 66 bytes

    /// <summary>
    /// Encrypts a plaintext string using XOR and encodes the result in Base64
    /// </summary>
    /// <param name="text">The plaintext to encrypt</param>
    /// <returns>The encrypted Base64 string</returns>
    public static string Encrypt(string text)
    {
        if(string.IsNullOrEmpty(text))
        {
            return text;
        }
        // Step 1: Convert string to bytes
        var bytes = System.Text.Encoding.UTF8.GetBytes(text);

        // Step 2: XOR each byte with the static key
        for ( int i = 0; i < bytes.Length; i++ )
        {
            bytes[i] = (byte) (bytes[i] ^ Key);
        }
        
        // Step 3: Convert encrypted bytes to Base64 for safe storage
        return Convert.ToBase64String(bytes);
    }
    
    /// <summary>
    /// Decrypts a Base64-encoded XOR-encrypted string
    /// </summary>
    /// <param name="encryptedText">The encrypted Base64 string</param>
    /// <returns>The decrypted plaintext</returns>
    public static string Decrypt(string encryptedText)
    {
        // Step 1: Convert Base64 back to raw encrypted bytes
        if(string.IsNullOrEmpty (encryptedText))
        {
            return encryptedText;
        }

        // Step 2: XOR again with the same key to reverse encryption 
        var bytes = Convert.FromBase64String(encryptedText);

        for(int i  = 0; i < bytes.Length;i++ )
        {
            bytes[i] = (byte)(bytes[i] ^ Key);
        }

        // Step 3: Convert bytes back into a string
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}