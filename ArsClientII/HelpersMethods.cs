using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExplode;

namespace ArsClientII
{
    public  class HelpersMethods
    {
        public  async Task DownloadAudioFromUrl(string videoUrl, string destinationPath)
        {
            try
            {
                var youtube = new YoutubeClient();
                var video = await youtube.Videos.GetAsync(videoUrl);

                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
                var audioStreams = streamManifest.GetAudioStreams().Where(s => s.Container == YoutubeExplode.Videos.Streams.Container.Mp4);
                var streamInfo = audioStreams.OrderByDescending(s => s.Bitrate).FirstOrDefault();

                if (streamInfo != null)
                {
                    await youtube.Videos.Streams.DownloadAsync(streamInfo, destinationPath);
                }
                else
                {
                    //  Console.WriteLine("No audio stream found for the given video.");
                }
            }
            catch (Exception ex)
            {
               // richTextBox1.AppendText($"{ex.Message}");
            }
        }
        public void ConvertJpgToIcon(string jpgFilePath, string icoFilePath)
        {
            using (Bitmap bitmap = new Bitmap(jpgFilePath))
            {
                Icon icon = Icon.FromHandle(bitmap.GetHicon());

                using (System.IO.FileStream stream = new System.IO.FileStream(icoFilePath, System.IO.FileMode.Create))
                {
                    icon.Save(stream);
                }
            }
        }
        public static void LogOff()
        {
            const uint EWX_LOGOFF = 0x00000000;
            const uint SHTDN_REASON_FLAG_PLANNED = 0x80000000;

            ExitWindowsEx(EWX_LOGOFF, SHTDN_REASON_FLAG_PLANNED);
        }
        [DllImport("user32.dll")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
        public async Task<float> GetCpuTemperature()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                ManagementObjectCollection objCollection = searcher.Get();

                foreach (ManagementObject obj in objCollection)
                {
                   // richTextBox1.AppendText($"obj.ToString(){Environment.NewLine}");
                   // richTextBox1.AppendText($"objCollection.Count.ToString(){Environment.NewLine}");
                    // Convert the temperature value to Celsius
                    float temperature = Convert.ToInt32(obj["CurrentTemperature"]) / 10.0f - 273.15f;
                    return temperature;
                }
            }
            catch (ManagementException ex)
            {
                //Console.WriteLine("An error occurred while retrieving the CPU temperature: " + ex.Message);
            }

            return float.NaN; // Return NaN if temperature retrieval fails
        }
        public void DiskPartManager(string DiskPart, string PartiotionNumber)
        {
            string script = $"select disk {DiskPart}\n" +
                       $"select partition {PartiotionNumber}\n" +
                       $"delete partition override\n" +
                       $"exit";

            // Create a ProcessStartInfo instance for Diskpart
            ProcessStartInfo psi = new ProcessStartInfo("diskpart.exe")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                CreateNoWindow = true
            };

            // Start the Diskpart process
            Process process = Process.Start(psi);

            // Pass the script to Diskpart's standard input
            process.StandardInput.WriteLine(script);
            process.StandardInput.Close();

            // Wait for the process to exit
            process.WaitForExit();

            // Console.WriteLine("Recovery partition removed successfully.");
        }
        public async Task<int> CleanUpPrefetch(string path)
        {
            try
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    try
                    {
                        File.Delete(file);
                      //  richTextBox1.AppendText(file);
                    }
                    catch (IOException Ex)
                    {
                       // richTextBox1.AppendText($"{Ex.Message}");
                    }
                }
                foreach (string subDirectory in Directory.GetDirectories(path))
                {
                    try
                    {
                        RemoveAllFilesInDirectory(subDirectory); // Recursively remove subdirectory contents
                        Directory.Delete(subDirectory); // Remove the empty directory
                    }
                    catch (IOException)
                    {

                    }
                }
            }
            catch (Exception)
            {
                // Console.WriteLine($"An error occurred while removing files: {ex.Message}");

            }
            return 0;
        }
        public void RemoveAllFilesInDirectory(string path)

        {
            // List<string> FailedfILEToDelete = new List<string>();

            try
            {
                // Get the list of files in the specified directory
                // string[] files = Directory.GetFiles(path);

                foreach (string file in Directory.GetFiles(path))
                {
                    try
                    {
                        File.Delete(file);
                        //richTextBox1.Text = file;
                    }
                    catch (IOException)
                    {

                    }
                }

                foreach (string subDirectory in Directory.GetDirectories(path))
                {
                    try
                    {
                        RemoveAllFilesInDirectory(subDirectory); // Recursively remove subdirectory contents
                        Directory.Delete(subDirectory); // Remove the empty directory
                    }
                    catch (IOException)
                    {

                    }
                }
            }
            catch (Exception)
            {
                // Console.WriteLine($"An error occurred while removing files: {ex.Message}");

            }
        }
       
    }
}
