using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class AesEncryption
{
    private static readonly byte[] Key =
        Encoding.UTF8.GetBytes("12345678901234567890123456789012"); //32 bit key

    private static readonly byte[] IV =
        Encoding.UTF8.GetBytes("1234567890123456"); //16 bit initialization vector to randomise the encrypt

    public static string Encrypt(string plainText)
    {
        using Aes aes = Aes.Create(); //create aes instance
        //assign key and iv
        aes.Key = Key;  
        aes.IV = IV;


        using MemoryStream ms = new MemoryStream(); //hold the encrypted bytes here
        using CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);    //encrypts stream of data
        using StreamWriter sw = new StreamWriter(cs);

        sw.Write(plainText); //sends the text thru the streamwriter ->goes thru crypto stream -> into memory
        sw.Close();

        return Convert.ToBase64String(ms.ToArray());    //encodes data
    }

    public static string Decrypt(string cipherText)
    {
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        byte[] buffer = Convert.FromBase64String(cipherText);

        using MemoryStream ms = new MemoryStream(buffer);
        using CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using StreamReader sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}
