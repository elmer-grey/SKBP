using System;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.InteropServices;

namespace Full_modul
{
    public static class SecurityHelper
    {
        public static string Encrypt(string plainText)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = ProtectedData.Protect(
                plainBytes,
                null,
                DataProtectionScope.CurrentUser
            );
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] plainBytes = ProtectedData.Unprotect(
                encryptedBytes,
                null,
                DataProtectionScope.CurrentUser
            );
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
