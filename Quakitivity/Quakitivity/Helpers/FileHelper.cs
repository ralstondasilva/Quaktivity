using System;
using static System.IO.Compression.ZipFile;
using System.Net;
using System.Threading.Tasks;
using System.IO;

namespace Quakitivity.Helpers
{
    class FileHelper
    {
        private static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create) + "\\Quakitivity";

        /// <summary>
        /// Downloads a file from the given url
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Path to downloaded file</returns>
        public static async Task<string> FetchFile(string url)
        {
            Directory.CreateDirectory(folderPath);

            string zipPath = folderPath + "\\WorldCities.zip";
            if (File.Exists(zipPath)) File.Delete(zipPath);

            WebClient webClient = new WebClient();
            await webClient.DownloadFileTaskAsync(new Uri(url), zipPath);

            return zipPath;
        }

        /// <summary>
        /// Extracts the contents of a zipped file
        /// </summary>
        /// <param name="zipPath"></param>
        /// <returns>Path to extracted file</returns>
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
