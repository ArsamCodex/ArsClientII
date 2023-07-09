
using System.Data;
using System.Diagnostics;
using YoutubeExplode;
using System.Management;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Media;
using Microsoft.VisualBasic.ApplicationServices;
using NAudio.Wave;

namespace ArsClientII
{
    public partial class Form1 : Form
    {
        string path = @"C:\Users\Armin\AppData\Local\Temp";
        string Prefetchpath = @"C:\Windows\Prefetch";
        string apiUrl = "https://api.coingecko.com/api/v3/coins/bitcoin/market_chart?vs_currency=usd&days=100";
        Color colorIncrease = Color.Green;
        Color colorDecrease = Color.Red;
        decimal previousPrice;
        private readonly ApplicationDbContext _context;
   

        private static SpeechRecognitionEngine recognizer;
        private static WaveOutEvent waveOutEvent;
        public Form1()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();
        }
       
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            recognizer.Dispose();
        }

      
        public void StopListening()
        {
            if (recognizer != null)
            {
                recognizer.Dispose();
                Console.WriteLine("Speech recognition stopped.");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Shutdown();
        }
        public void Shutdown()
        {
            Process.Start("shutdown", "/s /t 0");
        }
        public void Restart()
        {
            Process.Start("shutdown", "/r /t 0");
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "Please enter a value in textBox3.");
            }
            else
            {
                errorProvider1.Clear();

                richTextBox1.Text = "Please Wait Until U get Done";
                await DownloadAudioFromUrl(textBox1.Text, textBox2.Text);
                richTextBox1.AppendText(Environment.NewLine + "File Downloaded Successfully. Operation Done!");
                // richTextBox1.AppendText(Environment.NewLine + "File Downloaded Here: " + textBox4.Text);
            }

        }
        private async Task DownloadAudioFromUrl(string videoUrl, string destinationPath)
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
                //Console.WriteLine("An error occurred while downloading the audio: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string jpgFilePath = textBox1.Text;
            string icoFilePath = textBox2.Text;
            ConvertJpgToIcon(jpgFilePath, icoFilePath);
            richTextBox1.AppendText("ICO Extention Done");
        }
        static void ConvertJpgToIcon(string jpgFilePath, string icoFilePath)
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

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                // Specify the path to the text file
                string filePath = textBox1.Text;

                // Read the word to search from the TextBox
                string searchWord = textBox2.Text;

                // Create a StreamReader object to read the file
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    int count = 0;

                    // Read lines from the file and check for the search word
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Perform case-insensitive search
                        int occurrences = new Regex(Regex.Escape(searchWord), RegexOptions.IgnoreCase).Matches(line).Count;
                        richTextBox1.AppendText(line);
                        richTextBox1.AppendText("-----------------------");

                        count += occurrences;
                    }

                    // Display the count of occurrences to the user
                    // MessageBox.Show("The word '" + searchWord + "' appears " + count + " times in the file.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    richTextBox1.AppendText($"The word  {searchWord} appears  {count}  times in the file., Search Result");

                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("An error occurred while reading the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public System.Windows.Forms.Timer timer;
        private async void tabPage2_Click(object sender, EventArgs e)
        {
            try
            {
                await UpdatePrices(); // Call the method to update prices initially

                decimal movingAverage = await CalculateMovingAverage(apiUrl);
                string formattedMovingAverage = movingAverage.ToString("0.00");
                label8.Text = formattedMovingAverage;






                timer = new System.Windows.Forms.Timer(); // Initialize the timer
                timer.Interval = 15000; // Set the interval to 3 seconds
                timer.Tick += async (s, args) => await UpdatePrices(); // Assign the update method to the tick event
                timer.Start(); // Start the timer


            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the API request or timer setup
                //MessageBox.Show("An error occurred: " + ex.Message);
                richTextBox1.AppendText(ex.Message);
            }
        }
        private async Task UpdatePrices()
        {

            try
            {
                label7.Text = await GetBinancePrice("BTCUSDT");

                // label8.Text = await GetBinancePrice("BNBUSDT");

                previousPrice = decimal.Parse(await GetBinancePrice("BTCUSDT"));
                // previousPrice = decimal.Parse(label13.Text);



                // labelPrice.Text = label13.Text;

                // Compare the current price with the previous price and set the label color accordingly
                if (previousPrice < decimal.Parse(label7.Text))
                {
                    richTextBox1.AppendText("Down Down Down");
                    label8.BackColor = colorDecrease;
                }
                else if (previousPrice > decimal.Parse(label7.Text))
                {
                    label8.BackColor = colorIncrease;
                    richTextBox1.AppendText("UP UP UP");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the API request
                // MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        public async Task<string> GetBinancePrice(string symbol)
        {
            try
            {
                string apiUrl = $"https://www.binance.com/api/v3/ticker/price?symbol={symbol}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response and extract the price
                    JObject data = JObject.Parse(responseBody);
                    string price = (string)data["price"];

                    return price;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the API request
                richTextBox1.AppendText("No Internet Connetcio Valid" + ex.Message);
                return null;

            }
        }
        static async Task<decimal> CalculateMovingAverage(string apiUrl)
        {

            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync(apiUrl);
                CoinGeckoResponse data = JsonConvert.DeserializeObject<CoinGeckoResponse>(response);

                decimal[] prices = data.Prices.Select(p => p[1]).ToArray();
                decimal sum = prices.Sum();
                decimal movingAverage = sum / prices.Length;

                return movingAverage;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            var entities = _context.Information.Count();
            richTextBox1.AppendText(entities.ToString());
            var MyNewData = new Information();
            MyNewData.RestartCount = "547";
            MyNewData.ShutDownCount = "25420";
            _context.Information.Add(MyNewData);
            _context.SaveChanges();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Restart();
        }
        [DllImport("user32.dll")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        public static void LogOff()
        {
            const uint EWX_LOGOFF = 0x00000000;
            const uint SHTDN_REASON_FLAG_PLANNED = 0x80000000;

            ExitWindowsEx(EWX_LOGOFF, SHTDN_REASON_FLAG_PLANNED);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LogOff();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DiskPartManager(textBox1.Text, textBox2.Text);
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

            Console.WriteLine("Recovery partition removed successfully.");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            float cpuTemperature = GetCpuTemperature();

            if (!float.IsNaN(cpuTemperature))
            {
                //Console.WriteLine("CPU Temperature: " + cpuTemperature.ToString("0.00") + "°C");
                richTextBox1.AppendText($"CPU Temperature:  + {cpuTemperature.ToString("0.00")}+ °C");
                richTextBox1.AppendText("------------------------------" + Environment.NewLine);

            }
            else
            {
                //Console.WriteLine("Failed to retrieve CPU temperature.");
                richTextBox1.AppendText("Something Went Wrong");
            }

        }
        public float GetCpuTemperature()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                ManagementObjectCollection objCollection = searcher.Get();

                foreach (ManagementObject obj in objCollection)
                {
                    richTextBox1.AppendText(obj.ToString());
                    richTextBox1.AppendText("--------------");
                    richTextBox1.AppendText(objCollection.Count.ToString());
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

        private void tabPage3_Click(object sender, EventArgs e)
        {
            StartListening();
            richTextBox1.AppendText("Start Listenong wait 2 second");
        }
        public static void StartListening()
        {
            recognizer = new SpeechRecognitionEngine();
            recognizer.LoadGrammar(new Grammar(new GrammarBuilder("long live the king")));
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);

            Console.WriteLine("Listening for commands. Say 'Nakhla' to start Mehrdad hidden song");
        }
        private static async void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.Equals("long live the king", StringComparison.OrdinalIgnoreCase))
            {
                string musicFilePath = "C:\\Users\\Armin\\Desktop\\AudioDownloaded/MehrdadHidden-Nakhla.mp3";
                await PlayMusicAsync(musicFilePath);
            }
        }
        public static async Task PlayMusicAsync(string filePath)
        {
            try
            {
                using (var audioFileReader = new AudioFileReader(filePath))
                {
                    waveOutEvent = new WaveOutEvent();
                    waveOutEvent.Init(audioFileReader);
                    waveOutEvent.Play();

                    Console.WriteLine("Playing music: " + filePath);
                    Console.WriteLine("Press any key to stop playback.");
                    Console.ReadKey();

                    waveOutEvent.Stop();
                    waveOutEvent.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while playing the music: " + ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        

    }
    public class CoinGeckoResponse
    {
        [JsonProperty("prices")]
        public decimal[][] Prices { get; set; }
    }
}