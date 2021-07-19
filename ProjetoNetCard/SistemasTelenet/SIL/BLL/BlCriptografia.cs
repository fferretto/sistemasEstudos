using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using SIL.DAL;
using TELENET.SIL.PO;

namespace SIL.BLL
{
    public class BlCriptografia
    {

        readonly OPERADORA _fOperador;

        public BlCriptografia(OPERADORA operador)
        {
            _fOperador = operador;
        }


        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private static string BytesToHex(byte[] bytes, int len)
        {
            StringBuilder hexString = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                hexString.Append(bytes[i].ToString("X2"));
            }
            return hexString.ToString();
        }

        public static string Encrypt(string originalString)
        {            
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException
                       ("The string which needs to be encrypted can not be null.");
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            cryptoProvider.Mode = CipherMode.ECB;
            cryptoProvider.Key = encoding.GetBytes(originalString);//chave

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            return BytesToHex(memoryStream.GetBuffer(), 8);
        }

        public  string Criptografar(string paramTextoACriptografar)
        {
            daCriptografia daCrip = new daCriptografia(_fOperador);

            return daCrip.Criptografar(paramTextoACriptografar);
        }

        //public static string Decrypt(string cryptedString)
        //{
        //    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        //    if (String.IsNullOrEmpty(cryptedString))
        //    {
        //        throw new ArgumentNullException
        //           ("The string which needs to be decrypted can not be null.");
        //    }
        //    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        //    cryptoProvider.Mode = CipherMode.ECB;
        //    cryptoProvider.Key = encoding.GetBytes("12345678");

        //    cryptedString += new string('0', 496);
        //    MemoryStream memoryStream = new MemoryStream();
        //    CryptoStream cryptoStream = new CryptoStream(memoryStream,
        //        cryptoProvider.CreateDecryptor(), CryptoStreamMode.Write);
        //    cryptoStream.Write(StringToByteArray(cryptedString), 0, 8);
        //    cryptoStream.FlushFinalBlock();

        //    return System.Text.ASCIIEncoding.ASCII.GetString(memoryStream.GetBuffer());
        //}
    }    
}
