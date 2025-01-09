﻿using System.Security.Cryptography;
using System.Text;

namespace GM_Shop.Helper
{
    public static class PasswordHelper
    {
        public static string GenerateRandomKey(int length = 5)
        {
            var pattern = @"fksjifjeijkgjioewmmvnTUYOMBGYHCGJKLHJHJK*&^%%&%^%&!";
            var stringBuilder = new StringBuilder();
            var rd = new Random();

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return stringBuilder.ToString();
        }

        #region [Hashing Extension]
        public static string ToSHA256Hash(this string password, string? saltKey)
        {
            var sha256 = SHA256.Create();
            byte[] encryptedSHA256 = sha256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(password, saltKey)));
            sha256.Clear();

            return Convert.ToBase64String(encryptedSHA256);
        }

        public static string ToSHA512Hash(this string password, string? saltKey)
        {
            SHA512Managed sha512 = new SHA512Managed();
            byte[] encryptedSHA512 = sha512.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(password, saltKey)));
            sha512.Clear();

            return Convert.ToBase64String(encryptedSHA512);
        }

        public static string ToMd5Hash(this string password, string? saltKey)
        {
            using (var md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(password, saltKey)));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
        #endregion
    }
}

