//https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/
//https://stackoverflow.com/questions/17195969/generating-aes-256-bit-key-value?answertab=trending#tab-top

namespace Census.Classes;
public static class EncryptionHelper
{
  public static string Encrypt(string plainText, byte[] encryptionKeyBytes)
  {
    byte[] iv = new byte[16];
    byte[] array;

    using (Aes aes = Aes.Create())
    {
      aes.Key = encryptionKeyBytes;
      aes.IV = iv;

      ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

      using (MemoryStream memoryStream = new())
      {
        using (CryptoStream cryptoStream = new((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
        {
          using (StreamWriter streamWriter = new((Stream)cryptoStream))
          {
            streamWriter.Write(plainText);
          }

          array = memoryStream.ToArray();
        }
      }
    }

    return Convert.ToBase64String(array);
  }

  public static string Decrypt(string cipherText, byte[] encryptionKeyBytes)
  {
    byte[] iv = new byte[16];
    byte[] buffer = Convert.FromBase64String(cipherText);

    using (Aes aes = Aes.Create())
    {
      aes.Key = encryptionKeyBytes;
      aes.IV = iv;
      ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

      using (MemoryStream memoryStream = new(buffer))
      {
        using (CryptoStream cryptoStream = new((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
        {
          using (StreamReader streamReader = new((Stream)cryptoStream))
          {
            return streamReader.ReadToEnd();
          }
        }
      }
    }
  }

  private static readonly byte[] Salt = new byte[] { 10, 20, 30, 40, 50, 60, 70, 80 };
  public static byte[] CreateKey(string password, int keyBytes = 32)
  {
    const int Iterations = 300;
    var keyGenerator = new Rfc2898DeriveBytes(password, Salt, Iterations);
    return keyGenerator.GetBytes(keyBytes);
  }
}
