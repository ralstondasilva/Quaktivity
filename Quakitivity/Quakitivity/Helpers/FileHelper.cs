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
            //string zipPath = folderPath + "\\WorldCities.zip";
            //string extractPath = folderPath + "\\Quakitivity";
            Directory.CreateDirectory(folderPath);
            string zipPath = folderPath + "\\WorldCities.zip";
            if (File.Exists(zipPath)) File.Delete(zipPath);

            WebClient webClient = new WebClient();
            await webClient.DownloadFileTaskAsync(new Uri("http://www.opengeocode.org/download/worldcities.zip"), zipPath);

            //await ExtractFile(zipPath, extractPath);

            //return extractPath + "\\worldcities.csv";

            return zipPath;
        }

        public static async Task<string> ExtractFile(string zipPath)
        {
            //string extractPath = folderPath;
            Directory.CreateDirectory(folderPath);
            string filePath = folderPath + "\\worldcities.csv";
            if (File.Exists(filePath)) File. Delete(filePath);
            ExtractToDirectory(zipPath, folderPath);
            return filePath;
        }
    }
}
