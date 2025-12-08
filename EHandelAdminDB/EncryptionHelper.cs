namespace EHandelAdminDB;


//demo-klass som visar symmetrisk kryptering
//klartext > krypterar > lagra > läsa > dekryptera > klartext
public class EncryptionHelper
{
    // en väldigt grundläggande kryptering
    private const byte Key = 0x42; // 66 bytes

    public static string Encrypt(string text)
    {
        if(string.IsNullOrEmpty(text))
        {
            return text;
        }
        // steg 1: konvertera texten till bytes
        // varför? texten är Unicode (cgar/strings)
        //XOR för att kunna förvränga vår sträng och då behöver vi omvandla texten till en byte array
        var bytes = System.Text.Encoding.UTF8.GetBytes(text);

        // steg 2: En logisk operation, olika = 1, lika = 0
        // varför bytes[i] = (byte) (bytes[i] ^ Key)
        // - bytes[i] är en byte (0-255)
        // - Key är också en byte
        // - bytes[i] ^ Key ger ett int-resultat, så vi castar tillbaka till byte
        for ( int i = 0; i < bytes.Length; i++ )
        {
            bytes[i] = (byte) (bytes[i] ^ Key);
        }
        
        // steg 3: för att kunna spara resultatet som text.
        // Kodar vi bytes till Base64
        // Efter att vi har gjort XOR kan bytes innehålla obegripliga tecken för text/JSON
        return Convert.ToBase64String(bytes);
    }
    
    public static string Decrypt(string krypteradText)
    {
        // steg 1
        if(string.IsNullOrEmpty (krypteradText))
        {
            return krypteradText;
        }

        // steg 2
        // gör om Base64-strängen till bytes igen
        //XOR tillbaka med samma nyckel 
        var bytes = Convert.FromBase64String(krypteradText);

        for(int i  = 0; i < bytes.Length;i++ )
        {
            bytes[i] = (byte)(bytes[i] ^ Key);
        }

        // steg 3
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}