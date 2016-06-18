using System;
using System.Collections.Generic;
using static System.IO.Compression.ZipFile;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Quakitivity.Helpers
{
    class FileHelper
    {
        private static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create) + "\\Quakitivity";

        public static async Task<string> FetchFile()
        {
            Directory.CreateDirectory(folderPath);

            string zipPath = folderPath + "\\WorldCities.zip";
            if (File.Exists(zipPath)) File.Delete(zipPath);

            WebClient webClient = new WebClient();
            await webClient.DownloadFileTaskAsync(new Uri("http://www.opengeocode.org/download/worldcities.zip"), zipPath);

            return zipPath;
        }

        public static string ExtractFile(string zipPath)
        {
            Directory.CreateDirectory(folderPath);

            string filePath = folderPath + "\\worldcities.csv";
            if (File.Exists(filePath)) File.Delete(filePath);

            ExtractToDirectory(zipPath, folderPath);

            return filePath;
        }
    }
}
