using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Snowdrop.util
{
    class CryptographyUtil
    {
        private static string _IV = "cMwvFF48SIGq9t5I";
        private static string _Key = "E1b4BEzJnM0730Gv";
        private static string _Salt = "jkasdhjkashd";

        public static void EncryptFileAES(string inputFile, string outputFile)
        {
            AesManaged AES = new AesManaged();
            using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
            {
                AES.KeySize = MD5.HashSize;
                AES.BlockSize = MD5.HashSize;
                AES.IV = MD5.ComputeHash(Encoding.ASCII.GetBytes(_IV));
                AES.Key = MD5.ComputeHash(Encoding.ASCII.GetBytes(_Key));
            }

            using (FileStream reader = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            {
                using (FileStream writer = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (CryptoStream cs = new CryptoStream(writer, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        int bufferSize = 4096;
                        byte[] buffer = new byte[bufferSize];
                        int bytesRead;
                        do
                        {
                            bytesRead = reader.Read(buffer, 0, bufferSize);
                            if (bytesRead != 0)
                            {
                                cs.Write(buffer, 0, bytesRead);
                            }
                        }
                        while (bytesRead != 0);
                        cs.FlushFinalBlock();
                    }
                }
            }
        }

        public static void DecryptFileAES(string inputFile, string outputFile)
        {
            AesManaged AES = new AesManaged();
            using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
            {
                AES.KeySize = MD5.HashSize;
                AES.BlockSize = MD5.HashSize;
                AES.IV = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(_IV));
                AES.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(_Key));
            }
            using (FileStream reader = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            {
                using (FileStream writer = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (CryptoStream cs = new CryptoStream(reader, AES.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        int bufferSize = 4096;
                        byte[] buffer = new byte[bufferSize];
                        int bytesRead;
                        do
                        {
                            bytesRead = cs.Read(buffer, 0, bufferSize);
                            if (bytesRead != 0)
                            {
                                writer.Write(buffer, 0, bytesRead);
                            }
                        }
                        while (bytesRead != 0);
                    }
                }
            }
        }

        public static string DecryptStringAES(string cipherText)
        {
            string text;

            var aesAlg = NewRijndaelManaged();
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var cipher = Convert.FromBase64String(cipherText);

            using (var msDecrypt = new MemoryStream(cipher))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        text = srDecrypt.ReadToEnd();
                    }
                }
            }
            return text;
        }

        public static string EncryptStringAES(string input)
        {
            var aesAlg = NewRijndaelManaged();

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            var msEncrypt = new MemoryStream();

            using (var csEnc = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEnc = new StreamWriter(csEnc))
            {
                swEnc.Write(input);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        private static RijndaelManaged NewRijndaelManaged()
        {
            var saltBytes = Encoding.ASCII.GetBytes(_Salt);
            var key = new Rfc2898DeriveBytes(_Key, saltBytes);

            var aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

            return aesAlg;
        }

        /*public static string Md5(string text)
        {
            using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
            {
                byte[] hash = MD5.ComputeHash(Encoding.ASCII.GetBytes(text));

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }*/

        public static string Md5(byte[] input)
        {
            using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
            {
                byte[] hash = MD5.ComputeHash(input);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
