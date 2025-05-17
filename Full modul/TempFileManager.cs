using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Full_modul
{
    class TempFileManager
    {
        private static readonly string TempFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "TempDocuments");

        private static readonly List<string> TempFiles = new List<string>();

        static TempFileManager()
        {
            if (!Directory.Exists(TempFolder))
            {
                Directory.CreateDirectory(TempFolder);
                File.SetAttributes(TempFolder, File.GetAttributes(TempFolder) | FileAttributes.Hidden);
            }

            CleanTempFiles();
        }

        public static string GetTempFilePath(string fileName)
        {
            var tempPath = Path.Combine(TempFolder, Guid.NewGuid().ToString() + Path.GetExtension(fileName));
            TempFiles.Add(tempPath);
            return tempPath;
        }

        public static void CleanTempFiles()
        {
            try
            {
                foreach (var file in TempFiles)
                {
                    try { File.Delete(file); } catch { /* ignore */ }
                }
                TempFiles.Clear();

                foreach (var file in Directory.GetFiles(TempFolder))
                {
                    try { File.Delete(file); } catch { /* ignore */ }
                }
            }
            catch { /* ignore */ }
        }
    }
}
