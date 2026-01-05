//using System;
//using System.IO;
//using System.Text;
//using System.Security.Cryptography;
//using UnityEngine;

//public class Encrypttest : MonoBehaviour
//{
//    private void Start()
//    {
//        var testText = "Testing";
//        Debug.Log("Test Text: " + testText);
//        string encryptedData = CryptoUtility.Encrypt(testText);
//        Debug.Log("Encrypted Data: " + encryptedData);
//        string decryptedData = CryptoUtility.Decrypt(encryptedData);
//        Debug.Log("Decrypted Data: " + decryptedData);

//    }
//}

//public static class CryptoUtility
//{
//    // 32 bytes = 256-bit key
//    private static readonly byte[] Key = Encoding.UTF8.GetBytes("YOUR_SUPER_SECRET_32_BYTE_KEY!!");

//    // 16 bytes = 128-bit IV
//    private static readonly byte[] IV = Encoding.UTF8.GetBytes("16_BYTE_SECRETIV");

//    // EXACT sizes required by AES
//    private static readonly byte[] Key =
//        Encoding.UTF8.GetBytes("12345678901234567890123456789012"); // 32 bytes

//    private static readonly byte[] IV =
//        Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes

//    public static string Encrypt(string plainText)
//    {
//        using (Aes aes = Aes.Create())
//        {
//            aes.Key = Key;
//            aes.IV = IV;

//            using (var encryptor = aes.CreateEncryptor())
//            using (var ms = new MemoryStream())
//            {
//                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
//                using (var sw = new StreamWriter(cs))
//                {
//                    sw.Write(plainText);
//                }

//                return Convert.ToBase64String(ms.ToArray());
//            }
//        }
//    }

//    public static string Decrypt(string cipherText)
//    {
//        byte[] buffer = Convert.FromBase64String(cipherText);

//        using (Aes aes = Aes.Create())
//        {
//            aes.Key = Key;
//            aes.IV = IV;

//            using (var decryptor = aes.CreateDecryptor())
//            using (var ms = new MemoryStream(buffer))
//            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
//            using (var sr = new StreamReader(cs))
//            {
//                return sr.ReadToEnd();
//            }
//        }
//    }
//}

