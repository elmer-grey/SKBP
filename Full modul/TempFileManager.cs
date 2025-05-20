using System;
using System.Collections.Concurrent;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Full_modul
{
    class TempFileManager
    {
        private static readonly string TempFolder;

        private static readonly ConcurrentDictionary<string, string> _fileCache = new ConcurrentDictionary<string, string>();

        static TempFileManager()
        {
            string appDataFolder = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "HRCalculator");

            if (!Directory.Exists(appDataFolder))
            {
                Directory.CreateDirectory(appDataFolder);
                File.SetAttributes(appDataFolder, FileAttributes.Hidden);
            }

            TempFolder = Path.Combine(appDataFolder, "TempDocuments");
            if (!Directory.Exists(TempFolder))
            {
                Directory.CreateDirectory(TempFolder);
            }
            CleanTempFiles();
        }

        public static string GetTempFilePath(string serverFilePath)
        {
            // Генерируем хеш от оригинального пути для постоянного имени
            string fileHash = ComputeFileHash(serverFilePath);
            string extension = Path.GetExtension(serverFilePath);
            string tempFileName = $"{fileHash}{extension}";
            string tempFilePath = Path.Combine(TempFolder, tempFileName);

            _fileCache.TryAdd(serverFilePath, tempFilePath);
            return tempFilePath;
        }

        public static void CleanTempFiles()
        {
            try
            {
                foreach (var file in Directory.GetFiles(TempFolder))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { }
                }
            }
            catch { }
        }

        private static string ComputeFileHash(string filePath)
        {
            using (var sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(filePath.ToLower()));
                return BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 16);
            }
        }
    }
}