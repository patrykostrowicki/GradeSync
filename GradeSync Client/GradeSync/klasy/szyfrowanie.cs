using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;

public static class CryptoHelper
{
    private static string encryptionKey = GradeSync.Properties.Resources.klucz_aes;

    public static string Encrypt(string clearText)
    {
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            //generowanie losowej soli
            byte[] salt = new byte[8];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, salt);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                //dołącz sól do zaszyfrowanego tekstu
                byte[] encryptedData = ms.ToArray();
                byte[] encryptedDataWithSalt = new byte[salt.Length + encryptedData.Length];
                Array.Copy(salt, 0, encryptedDataWithSalt, 0, salt.Length);
                Array.Copy(encryptedData, 0, encryptedDataWithSalt, salt.Length, encryptedData.Length);

                return Convert.ToBase64String(encryptedDataWithSalt);
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        byte[] cipherBytesWithSalt = Convert.FromBase64String(cipherText);

        //ekstrakcja soli z zaszyfrowanych danych
        byte[] salt = new byte[8];
        Array.Copy(cipherBytesWithSalt, 0, salt, 0, salt.Length);
        byte[] cipherBytes = new byte[cipherBytesWithSalt.Length - salt.Length];
        Array.Copy(cipherBytesWithSalt, salt.Length, cipherBytes, 0, cipherBytes.Length);

        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, salt);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                return Encoding.Unicode.GetString(ms.ToArray());
            }
        }
    }
}

