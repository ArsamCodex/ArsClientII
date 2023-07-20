using System.Data;
using System.Diagnostics;
using YoutubeExplode;
using System.Management;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NAudio.Wave;
using NAudio.MediaFoundation;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using AngleSharp.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Text;
using System.Net;
using Microsoft.VisualBasic.ApplicationServices;
using HtmlAgilityPack;
using System.Security.Cryptography;
using System.Net.Http;
using System;

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
        private const string Url = "https://cryptodaily.co.uk/";
        private readonly ApplicationDbContext _context;
        private static bool isPlaying = false;
        private static bool isSilent = false;
        private static WaveOutEvent waveOutEvent;
        SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
        private OkexApiClient _okexApiClient;
        private const string ApiKey = "fcf7293a-dc47-4b54-8ceb-deaedbb7f7d7";
        private const string SecretKey = "6364844206A41E7F389C365C9480211E";
        private const string BaseUrl = "https://www.okex.com";
        public Form1()
        {
            InitializeComponent();
            _okexApiClient = new OkexApiClient();
            _context = new ApplicationDbContext();
        }




        static async Task<decimal> CalculateMovingAverage(string apiUrl, int timeIntervalInMinutes)
        {
            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync(apiUrl);
                CoinGeckoResponse data = JsonConvert.DeserializeObject<CoinGeckoResponse>(response);

                // Assuming data.Prices is an array of [timestamp, price]
                // Convert timestamp to DateTime and sort by timestamp
                var sortedData = data.Prices
                    .Select(p => new { TimeStamp = DateTimeOffset.FromUnixTimeMilliseconds((long)p[0]).UtcDateTime, Price = p[1] })
                    .OrderBy(p => p.TimeStamp)
                    .ToList();

                // Calculate the number of data points within the desired time interval
                int dataPointsWithinInterval = timeIntervalInMinutes / 5; // Assuming the API returns data every 5 minutes
                if (dataPointsWithinInterval == 0)
                {
                    throw new ArgumentException("Time interval should be a multiple of 5 minutes.");
                }

                // Calculate the moving average for the given time interval
                List<decimal> movingAverages = new List<decimal>();
                decimal sum = 0;
                for (int i = 0; i < sortedData.Count; i++)
                {
                    sum += sortedData[i].Price;

                    // If the number of data points considered for the moving average equals the desired interval,
                    // calculate and store the moving average for this interval
                    if (i >= dataPointsWithinInterval - 1)
                    {
                        decimal movingAverage = sum / dataPointsWithinInterval;
                        movingAverages.Add(movingAverage);

                        // Move the window by removing the oldest data point
                        sum -= sortedData[i - dataPointsWithinInterval + 1].Price;
                    }
                }

                // The movingAverages list now contains the 15-minute moving averages for each interval in chronological order.
                // You can use this data as needed.

                // For example, if you want to get the most recent 15-minute moving average:
                decimal last15MinMovingAverage = movingAverages.Last();

                return last15MinMovingAverage;
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
                richTextBox1.AppendText("File Downloaded Successfully. Operation Done!" + Environment.NewLine);
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
                richTextBox1.AppendText($"{ex.Message}");
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
                        richTextBox1.AppendText($"{line}{Environment.NewLine}");


                        count += occurrences;
                    }

                    // Display the count of occurrences to the user
                    // MessageBox.Show("The word '" + searchWord + "' appears " + count + " times in the file.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    richTextBox1.AppendText($"The word  {searchWord} appears  {count}  times in the file., Search Result{Environment.NewLine}");

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
                richTextBox1.AppendText($"{ex.Message}{Environment.NewLine}");
            }
        }
        private async Task UpdatePrices()
        {
            try
            {
                label7.Text = await GetBinancePrice("BTCUSDT");
                previousPrice = decimal.Parse(await GetBinancePrice("BTCUSDT"));
                if (previousPrice < decimal.Parse(label7.Text))
                {
                    richTextBox1.AppendText($"Down Down Down{Environment.NewLine}");
                    label8.BackColor = colorDecrease;
                }
                else if (previousPrice > decimal.Parse(label7.Text))
                {
                    label8.BackColor = colorIncrease;
                    richTextBox1.AppendText($"UP UP UP{Environment.NewLine}");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the API request
                richTextBox1.AppendText($"{ex.Message}");
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
                richTextBox1.AppendText($"No Internet Connetcio Valid{ex.Message}{Environment.NewLine}");
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
            _context.Dispose();
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

            // Console.WriteLine("Recovery partition removed successfully.");
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            float cpuTemperature = await GetCpuTemperature();

            if (!float.IsNaN(cpuTemperature))
            {
                //Console.WriteLine("CPU Temperature: " + cpuTemperature.ToString("0.00") + "°C");
                richTextBox1.AppendText($"CPU Temperature:  + {cpuTemperature.ToString("0.00")}+ °C{Environment.NewLine}");
            }
            else
            {
                //Console.WriteLine("Failed to retrieve CPU temperature.");
                richTextBox1.AppendText($"Something Went Wrong exception line 361{Environment.NewLine}");
            }
        }
        public async Task<float> GetCpuTemperature()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                ManagementObjectCollection objCollection = searcher.Get();

                foreach (ManagementObject obj in objCollection)
                {
                    richTextBox1.AppendText($"obj.ToString(){Environment.NewLine}");
                    richTextBox1.AppendText($"objCollection.Count.ToString(){Environment.NewLine}");
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

        private async Task<List<string>> GetH3Headers(string url)
        {
            List<string> headers = new List<string>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string pageContent = await response.Content.ReadAsStringAsync();

                    // Find all the h3 tags using regular expression

                    // string pattern = "<div class=\"news-item\">(.*?)<h2>(.*?)</h2>(.*?)<span class=\"date\">(.*?)</span>";


                    string pattern = @"<h3\b[^>]*>(.*?)</h3>";
                    MatchCollection matches = Regex.Matches(pageContent, pattern, RegexOptions.IgnoreCase);

                    // Extract the header texts
                    foreach (Match match in matches)
                    {
                        string header = RemoveHtmlTags(match.Groups[1].Value.Trim());
                        headers.Add(header);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return headers;
        }

        string FrenchTranslated;
        public string CoreTranslation(string EnglishWord)
        {
            Dictionary<string, string> translationMap = new Dictionary<string, string>()
    {
        { "accident", "TestBro" },
        { "bye", "au revoir" }
    };

            if (translationMap.ContainsKey(EnglishWord))
            {
                return translationMap[EnglishWord];
            }
            else
            {
                return "Translation not found";
            }
        }
        static async Task<string> MoreDynamic(string incomingText)
        {
            string? finalResultToReturn = null;
            List<string>? MyIntroduceSentencess = new List<string>();
            //in sad cases
            MyIntroduceSentencess.Add("Oh i Am Sorry for that ,why you had bad day , how can i make your day number 40");
            //in cases of ha[piness
            MyIntroduceSentencess.Add("what nice .............");
            //free case
            MyIntroduceSentencess.Add("what the fuck");
            MyIntroduceSentencess.Add("welcome guest ");

            bool CheckOrEcists = incomingText.Contains("sad") || incomingText.Contains("number 40");
            if (CheckOrEcists)
            {
                finalResultToReturn = MyIntroduceSentencess.ElementAt(0).ToString();
                return finalResultToReturn;
            }
            bool CheckOrEcists2 = incomingText.Contains("nice");
            if (CheckOrEcists2)
            {
                finalResultToReturn = MyIntroduceSentencess.ElementAt(1).ToString();
                return finalResultToReturn;
            }
            bool CheckOrEcists3 = incomingText.Contains("nice");
            if (CheckOrEcists3)
            {
                finalResultToReturn = MyIntroduceSentencess.ElementAt(2).ToString();
                return finalResultToReturn;
            }
            bool CheckOrEcists4 = incomingText.Contains("guest");
            if (CheckOrEcists4)
            {
                finalResultToReturn = MyIntroduceSentencess.ElementAt(3).ToString();
                return finalResultToReturn;
            }

            return finalResultToReturn;
        }
        private async void tabPage3_Click(object sender, EventArgs e)
        {
            string mainText = "Voice Command Activated . . .";
            ReadText($"Voice Command Activated . I am  listening . Today Is:  {DateTime.Now.Date} ");
            label15.Text = mainText;
            // Set the input language to English
            recognizer.SetInputToDefaultAudioDevice();
            // Define grammar choices
            var choices = new Choices(
  "cleaning", "happy walking sir, i am waiting you here", "i am goin to walk", "Chavoshi", "i am eating", "bit coin", "number 1", "richbox",
  "247",
  "my name is armin nice to meet you", "do you like talk to me ?", "Tupac", "Vigen",
  "Arsam", "Stop", "Shutthefuckoff", "Mehrdad", "how sexy you are", "pedarsag", "two nine six",
  "able", "about", "above", "accept", "accident", "account", "accurate", "across", "act", "active",
  "actual", "add", "address", "admire", "admit", "advice", "afraid", "after", "afternoon", "again",
  "age", "agree", "air", "all", "allow", "almost", "alone", "along", "already", "also", "although",
  "always", "amazing", "among", "amount", "ancient", "anger", "animal", "answer", "anxiety", "any",
  "apart", "apparent", "appear", "apply", "approach", "approve", "area", "argue", "arm", "around",
  "arrange", "arrest", "arrive", "art", "ask", "aspect", "assault", "assert", "assess", "assign",
  "assist", "assume", "atmosphere", "attach", "attack", "attempt", "attend", "attract", "auction",
  "avoid", "award", "away", "awful", "awkward", "baby", "back", "bad", "balance", "ball", "ban",
  "band", "bank", "bar", "barely", "bargain", "barrier", "base", "basic", "basket", "battle",
  "beach", "beam", "bear", "beat", "beautiful", "become", "beef", "before", "begin", "behave",
  "behind", "believe", "bell", "belong", "below", "belt", "bench", "benefit", "besides", "best",
  "bet", "better", "between", "beyond", "big", "bike", "bind", "biology", "bird", "birth", "bitter",
  "black", "blade", "blame", "blanket", "blast", "bleed", "blend", "bless", "blind", "block", "blood",
  "blow", "blue", "board", "boat", "body", "bomb", "bond", "bone", "bonus", "book", "boost", "border",
  "boring", "borrow", "boss", "bottom", "bounce", "box", "boy", "brave", "bread", "break", "breeze",
  "brick", "bridge", "brief", "bright", "bring", "brisk", "brother", "brown", "brush", "bubble",
  "bucket", "budget", "build", "bulb", "bulk", "bullet", "bunch", "burden", "burger", "burn",
  "burst", "bus", "business", "busy", "butter", "buyer", "buzz", "cabbage", "cabin", "cable",
  "cage", "cake", "call", "calm", "camera", "camp", "can", "cancel", "candy", "cannon", "canoe",
  "canvas", "canyon", "capable", "capital", "captain", "car", "carbon", "card", "care", "cargo",
  "carpet", "carry", "cart", "case", "cash", "casino", "castle", "casual", "cat", "catalog",
  "catch", "category", "cattle", "caught", "cause", "caution", "cave", "ceiling", "celery", "cell",
  "center", "century", "ceremony", "certain", "chair", "chalk", "champion", "change", "chaos",
  "chapter", "charge", "chase", "chat", "cheap", "check", "cheese", "chef", "cherry", "chest",
  "chicken", "chief", "child", "chimney", "choice", "choose", "chronic", "chuckle", "chunk",
  "churn", "cigar", "cinnamon", "circle", "citizen", "city", "civil", "claim", "clap", "clarify",
  "claw", "clay", "clean", "clerk", "clever", "click", "client", "cliff", "climb", "clinic", "clip",
  "clock", "clog", "close", "cloth", "cloud", "clown", "club", "clump", "cluster", "clutch", "coach",
  "coast", "coconut", "code", "coffee", "coil", "coin", "collect", "color", "column", "combine",
  "come", "comfort", "comic", "common", "company", "concert", "conduct", "confirm", "congress",
  "connect", "consider", "control", "convince", "cook", "cool", "copper", "copy", "coral", "core", "100",
  "corn", "correct", "cost", "cotton", "couch", "country", "couple", "course", "cousin", "cover",
  "coyote", "crack", "cradle", "craft", "cram", "crane", "crash", "crater", "crawl", "crazy", "cream",
  "credit", "creek", "crew", "cricket", "crime", "crisp", "critic", "crop", "cross", "crouch", "crowd",
  "crucial", "cruel", "cruise", "crumble", "crunch", "crush", "cry", "crystal", "cube", "culture",
  "cup", "cupboard", "curious", "current", "curtain", "curve", "cushion", "custom", "cute", "cycle",
  "dad", "damage", "damp", "dance", "danger", "daring", "dash", "daughter", "dawn", "day", "deal",
  "debate", "debris", "decade", "december", "decide", "decline", "decorate", "decrease", "deer",
  "defense", "define", "defy", "degree", "delay", "deliver", "demand", "demise", "denial", "dentist",
  "deny", "depart", "depend", "deposit", "depth", "deputy", "derive", "describe", "desert", "design",
  "desk", "despair", "destroy", "detail", "detect", "develop", "device", "devote", "diagram", "dial",
  "diamond", "diary", "dice", "diesel", "diet", "differ", "digital", "dignity",
  "each", "eager", "eagle", "early", "earn", "earth", "easily", "east", "easy", "echo", "ecology", "edge",
  "edit", "educate", "effort", "egg", "eight", "either", "elbow", "elder", "electric", "elegant", "element",
  "elephant", "elevator", "elite", "else", "embark", "embody", "embrace", "emerge", "emotion", "employ",
  "empty", "enable", "enact", "end", "endless", "endorse", "enemy", "energy", "enforce", "engage", "engine",
  "enhance", "enjoy", "enlist", "enough", "enrich", "enroll", "ensure", "enter", "entire", "entry", "envelope",
  "episode", "equal", "equip", "era", "erase", "erode", "erosion", "error", "erupt", "escape", "essay", "essence",
  "estate", "eternal", "ethics", "evidence", "evil", "evoke", "evolve", "exact", "example", "excess", "exchange",
  "excite", "exclude", "excuse", "execute", "exercise", "exhaust", "exhibit", "exile", "exist", "exit", "exotic",
  "expand", "expect", "expire", "explain", "expose", "express", "extend", "extra", "eye", "eyebrow", "fabric",
  "face", "faculty", "fade", "faint", "faith", "fall", "false", "fame", "family", "famous", "fan", "fancy",
  "fantasy", "farm", "fashion", "fat", "fatal", "father", "fatigue", "fault", "favorite", "feature", "february",
  "federal", "fee", "feed", "feel", "female", "fence", "festival", "fetch", "fever", "few", "fiber", "fiction",
  "field", "figure", "file", "film", "filter", "final", "find", "fine", "finger", "finish", "fire", "firm",
  "first", "fiscal", "fish", "fit", "fitness", "fix", "flag", "flame", "flash", "flat", "flavor", "flee",
  "flight", "flip", "float", "flock", "floor", "flower", "fluid", "flush", "fly", "foam", "focus", "fog",
  "foil", "fold", "follow", "food", "foot", "force", "forest", "forget", "fork", "fortune", "forum", "forward",
  "fossil", "foster", "found", "fox", "fragile", "frame", "frequent", "fresh", "friend", "fringe", "frog",
  "front", "frost", "frown", "frozen", "fruit", "fuel", "fun", "funny", "furnace", "fury", "future", "gadget",
  "gain", "galaxy", "gallery", "game", "gap", "garage", "garbage", "garden", "garlic", "garment", "gas",
  "gasp", "gate", "gather", "gauge", "gaze", "general", "genius", "genre", "gentle", "genuine", "gesture", "fantom",
  "ghost", "giant", "gift", "giggle", "ginger", "giraffe", "girl", "give", "glad", "glance", "glare", "glass",
  "glide", "glimpse", "globe", "gloom", "glory", "glove", "glow", "glue", "goat", "goddess", "gold", "good",
  "goose", "gorilla", "gospel", "gossip", "govern", "gown", "grab", "grace", "grain", "grant", "grape",
  "grass", "gravity", "great", "green", "grid", "grief", "grit", "grocery", "group", "grow", "grunt", "guard",
  "guess", "guide", "guilt", "guitar", "gun", "gym", "habit", "hair", "half", "hammer", "hamster", "hand",
  "happy", "harbor", "hard", "harsh", "harvest", "hat", "have", "hawk", "hazard", "head", "health", "heart",
  "heavy", "hedgehog", "height", "hello", "helmet", "help", "hen", "hero", "hidden", "high", "hill", "hint",
  "hip", "hire", "history", "hobby", "hockey", "hold", "hole", "holiday", "hollow", "home", "honey", "hood",
  "hope", "horn", "horror", "horse", "hospital", "host", "hotel", "hour", "hover", "hub", "huge", "human",
  "humble", "humor", "hundred", "hungry", "hunt", "hurdle", "hurry", "hurt", "husband", "hybrid", "ice", "icon",
  "idea", "identify", "idle", "ignore", "ill", "illegal", "illness", "image", "imitate", "immense", "immune",
  "impact", "impose", "improve", "impulse", "inch", "include", "income", "increase", "index", "indicate",
  "indoor", "industry", "infant", "inflict", "inform", "inhale", "inherit", "initial", "inject", "injury",
  "inmate", "inner", "innocent", "input", "inquiry", "insane", "insect", "inside", "inspire", "install",
  "intact", "interest", "into", "invest", "invite", "involve", "iron", "island", "isolate", "issue", "item",
  "ivory", "jacket", "jaguar", "jar", "jazz", "jealous", "jeans", "jelly", "jewel", "job", "join", "joke",
  "journey", "joy", "judge", "juice", "jump", "jungle", "junior", "junk", "just", "kangaroo", "keen", "keep",
  "ketchup", "key", "kick",
  "jackal", "jacket", "jaguar", "jam", "janitor", "jar", "jazz", "jealous", "jeans", "jelly", "jewel",
  "job", "join", "joke", "journey", "joy", "judge", "juice", "jump", "jungle", "junior", "junk", "just",
  "kangaroo", "keen", "keep", "ketchup", "key", "kick", "kid", "kidney", "kind", "kingdom", "kiss", "kit",
  "kitchen", "kite", "kitten", "kiwi", "knee", "knife", "knock", "know", "lab", "label", "labor", "ladder",
  "lady", "lake", "lamp", "language", "laptop", "large", "later", "latin", "laugh", "laundry", "lava", "law",
  "lawn", "lawsuit", "layer", "lazy", "leader", "leaf", "learn", "leave", "lecture", "left", "leg", "legal",
  "legend", "leisure", "lemon", "lend", "length", "lens", "leopard", "lesson", "letter", "level", "liar",
  "liberty", "library", "license", "life", "lift", "light", "like", "limb", "limit", "link", "lion", "liquid",
  "list", "little", "live", "lizard", "load", "loan", "lobster", "local", "lock", "logic", "lonely", "long",
  "loop", "lottery", "loud", "lounge", "love", "loyal", "lucky", "luggage", "lumber", "lunar", "lunch",
  "luxury", "lyrics", "machine", "mad", "magic", "magnet", "maid", "mail", "main", "major", "make", "mammal", "number 2",
  "man", "manage", "mandate", "mango", "mansion", "manual", "maple", "marble", "march", "margin", "marine",
  "market", "marriage", "mask", "mass", "master", "match", "material", "math", "matrix", "matter", "maximum",
  "maze", "meadow", "mean", "measure", "meat", "mechanic", "medal", "media", "melody", "melt", "member",
  "memory", "mention", "menu", "mercy", "merge", "merit", "merry", "mesh", "message", "metal", "method",
  "middle", "midnight", "milk", "million", "mimic", "mind", "minimum", "minor", "minute", "miracle", "mirror",
  "misery", "miss", "mistake", "mix", "mixed", "mixture", "mobile", "model", "modify", "mom", "moment",
  "monitor", "monkey", "monster", "month", "moon", "moral", "more", "morning", "mosquito", "mother", "motion",
  "motor", "mountain", "mouse", "move", "movie", "much", "muffin", "mule", "multiply", "muscle", "museum",
  "mushroom", "music", "must", "mutual", "myself", "mystery", "myth", "naive", "name", "napkin", "narrow",
  "nasty", "nation", "nature", "near", "neck", "need", "negative", "neglect", "neither", "nephew", "nerve",
  "nest", "net", "network", "neutral", "never", "news", "next", "nice", "night", "noble", "noise", "nominee",
  "noodle", "normal", "north", "nose", "notable", "note", "nothing", "notice", "novel", "now", "nuclear",
  "number", "nurse", "nut", "oak", "obey", "object", "oblige", "obscure", "observe", "obtain", "obvious",
  "occur", "ocean", "october", "odor", "off", "offer", "office", "often", "oil", "okay", "old", "olive",
  "olympic", "omit", "once", "one", "onion", "online", "only", "open", "opera", "opinion", "oppose", "option",
  "orange", "orbit", "orchard", "order", "ordinary", "organ", "orient", "original", "orphan", "ostrich",
  "other", "outdoor", "outer", "output", "outside", "oval", "oven", "over", "own", "owner", "oxygen", "oyster",
  "ozone", "pact", "paddle", "page", "pair", "palace", "palm", "panda", "panel", "panic", "panther", "paper",
  "parade", "parent", "park", "parrot", "party", "pass", "patch", "path", "patient", "patrol", "pattern",
  "pause", "pave", "payment", "peace", "peanut", "pear", "peasant", "pelican", "pen", "penalty", "pencil",
  "people", "pepper", "perfect", "permit", "person", "pet", "phone", "photo", "phrase", "physical", "piano",
  "picnic", "picture", "piece", "pig", "pigeon", "pill", "pilot", "pink", "pioneer", "pipe", "pistol", "pitch",
  "pizza", "place", "planet", "plastic", "plate", "play", "please", "pledge", "pluck", "plug", "plunge",
  "poem", "poet", "point", "polar", "pole", "police", "pond", "pony", "pool", "popular", "portion", "position",
  "possible", "post", "potato", "pottery", "poverty", "powder", "power", "practice", "praise", "predict",
  "prefer", "prepare", "present", "pretty", "prevent", "price", "pride", "primary", "print", "priority",
  "prison", "private", "prize", "problem", "process", "produce", "profit", "program", "project",
  "quality", "quantum", "quarter", "queen", "query", "quest", "quick", "quiet", "quilt", "quit", "quiz", "2",
  "quote", "rabbit", "race", "rack", "radar", "radio", "rail", "rain", "raise", "rally", "ramp", "ranch",
  "random", "range", "rapid", "rare", "rate", "rather", "raven", "raw", "razor", "ready", "real", "reason",
  "rebel", "rebuild", "recall", "receive", "recipe", "record", "recycle", "reduce", "reflect", "reform",
  "refuse", "region", "regret", "regular", "reject", "relax", "release", "relief", "rely", "remain",
  "remember", "remind", "remove", "render", "renew", "rent", "reopen", "repair", "repeat", "replace",
  "report", "require", "rescue", "resemble", "resist", "resource", "response", "result", "retire",
  "retreat", "return", "reunion", "reveal", "review", "reward", "rhythm", "rib", "ribbon", "rice", "rich",
  "ride", "ridge", "rifle", "right", "rigid", "ring", "riot", "ripple", "risk", "ritual", "rival", "river",
  "road", "roast", "robot", "robust", "rocket", "romance", "roof", "rookie", "room", "rose", "rotate",
  "rough", "round", "route", "royal", "rubber", "rude", "rug", "rule", "run", "runway", "rural", "sad",
  "saddle", "sadness", "safe", "sail", "salad", "salmon", "salon", "salt", "salute", "same", "sample",
  "sand", "satisfy", "satoshi", "sauce", "sausage", "save", "say", "scale", "scan", "scare", "scatter",
  "scene", "scheme", "school", "science", "scissors", "scorpion", "scout", "scrap", "screen", "script",
  "scrub", "sea", "search", "season", "seat", "second", "secret", "section", "security", "seed", "seek",
  "segment", "select", "sell", "seminar", "senior", "sense", "sentence", "series", "service", "session",
  "settle", "setup", "seven", "shadow", "shaft", "shallow", "share", "shed", "shell", "sheriff", "shield",
  "shift", "shine", "ship", "shiver", "shock", "shoe", "shoot", "shop", "short", "shoulder", "shove",
  "shrimp", "shrug", "shuffle", "shy", "sibling", "sick", "side", "siege", "sight", "sign", "silent",
  "silk", "silly", "silver", "similar", "simple", "since", "sing", "siren", "sister", "situate", "six",
  "size", "skate", "sketch", "ski", "skill", "skin", "skirt", "skull", "slab", "slam", "sleep", "slender",
  "slice", "slide", "slight", "slim", "slogan", "slot", "slow", "slush", "small", "smart", "smile", "smoke",
  "smooth", "snack", "snake", "snap", "sniff", "snow", "soap", "soccer", "social", "sock", "soda", "soft",
  "solar", "soldier", "solid", "solution", "solve", "someone", "song", "soon", "sorry", "sort", "soul",
  "sound", "soup", "source", "south", "space", "spare", "spatial", "spawn", "speak", "special", "speed",
  "spell", "spend", "sphere", "spice", "spider", "spike", "spin", "spirit", "split", "spoil", "sponsor",
  "spoon", "sport", "spot", "spray", "spread", "spring", "spy", "square", "squeeze", "squirrel", "stable",
  "stadium", "staff", "stage", "stairs", "stamp", "stand", "start", "state", "stay", "steak", "steel", "i am very sad today",
  "stem", "step", "stereo", "stick", "still", "sting", "stock", "stomach", "stone", "stool", "story", "star",
  "stove", "strategy", "street", "strike", "strong", "struggle", "student", "stuff", "stumble", "style",
  "subject", "submit", "subway", "success", "such", "sudden", "suffer", "sugar", "suggest", "suit", "summer",
  "sun", "sunny", "sunset", "super", "supply", "supreme", "sure", "surface", "surge", "surprise", "surround",
  "survey", "suspect", "sustain", "swallow", "swamp", "swap", "swarm", "swear", "sweet", "swift", "swim",
  "swing", "switch", "sword", "symbol", "symptom", "syrup", "system",
  "table", "tackle", "tag", "tail", "talent", "talk", "tank", "tape", "target", "task", "taste", "tattoo",
  "taxi", "teach", "team", "tell", "ten", "tenant", "tennis", "tent", "term", "test", "text", "thank", "that",
  "theme", "then", "theory", "there", "they", "thing", "this", "thought", "three", "thrive", "throw", "thumb",
  "thunder", "ticket", "tide", "tiger", "tilt", "timber", "time", "tiny", "tip", "tired", "tissue", "title",
  "toast", "tobacco", "today", "toddler", "toe", "together", "toilet", "token", "tomato", "tomorrow", "tone",
  "tongue", "tonight", "tool", "tooth", "top", "topic", "topple", "torch", "tornado", "tortoise", "toss",
  "total", "tourist", "toward", "tower", "town", "toy", "track", "trade", "traffic", "tragic", "train",
  "transfer", "trap", "trash", "travel", "tray", "treat", "tree", "trend", "trial", "tribe", "trick", "trigger",
  "trim", "trip", "trophy", "trouble", "truck", "true", "truly", "trumpet", "trust", "truth", "try", "tube",
  "tuition", "tumble", "tuna", "tunnel", "turkey", "turn", "turtle", "twelve", "twenty", "twice", "twin",
  "twist", "two", "type", "typical", "ugly", "umbrella", "unable", "unaware", "uncle", "uncover", "under",
  "undo", "unfair", "unfold", "unhappy", "uniform", "unique", "unit", "universe", "unknown", "unlock",
  "until", "unusual", "unveil", "update", "upgrade", "uphold", "upon", "upper", "upset", "urban", "urge",
  "usage", "use", "used", "useful", "useless", "usual", "utility", "vacant", "vacuum", "vague", "valid",
  "valley", "valve", "van", "vanish", "vapor", "various", "vast", "vault", "vehicle", "velvet", "vendor",
  "venture", "venue", "verb", "verify", "version", "very", "vessel", "veteran", "viable", "vibrant", "vicious",
  "victory", "video", "view", "village", "vintage", "violin", "virtual", "virus", "visa", "visit", "visual",
  "vital", "vivid", "vocal", "voice", "void", "volcano", "volume", "vote", "voyage", "wage", "wagon", "wait",
  "walk", "wall", "walnut", "want", "warfare", "warm", "warrior", "wash", "wasp", "waste", "water", "wave",
  "way", "wealth", "weapon", "wear", "weasel", "weather", "web", "wedding", "week", "weird", "welcome",
  "west", "wet", "whale", "what", "wheat", "wheel", "when", "where", "whip", "whisper", "wide", "width",
  "wife", "wild", "will", "win", "window", "wine", "wing", "wink", "winner", "winter", "wire", "wisdom",
  "wise", "wish", "witness", "wolf", "woman", "wonder", "wood", "wool", "word", "work", "world", "worry",
  "worth", "wrap", "wreck", "wrestle", "wrist", "write", "wrong", "yard", "year", "yellow", "you", "young",
  "youth", "zebra", "zero", "zone", "zoo", "i am very fine today", "what is news", "stop 1", "i am back", "kill your self", "i am sleepy", "number 40",
  "track 1", "50", "Tatal", "you are my lovely client", "thanks", "fifty", "number fifty",
  "hey shadow i introduce you my guest", "guest", "hey this is stranger", "stranger", "this is my guest", "shadow", "hey shadow",
  "thank you", "goriz", "coingraph", "CryptoNews", "naaz", "Tupac", "crypto daily", "number 6", "pac",
  "tupac", "Tupac","Pocheel","number 4");


            // Create a grammar from the choices
            var grammar = new Grammar(new GrammarBuilder(choices));

            // Load the grammar
            recognizer.LoadGrammar(grammar);

            recognizer.RecognizeAsync(RecognizeMode.Multiple);

            //Console.WriteLine("Speak something...");
            richTextBox1.AppendText($"Say Somethign{Environment.NewLine}");

            // Handle speech recognition events
            recognizer.SpeechRecognized += async (sender, e) =>
            {
                if (e.Result.Confidence >= 0.7) // Adjust confidence threshold as needed
                {
                    // Console.WriteLine("You said: " + e.Result.Text);
                    //  richTextBox1.AppendText($" {e.Result.Text}  ");
                    label16.Text = e.Result.Text;
                    if (isPlaying)
                    {
                        if (e.Result.Text.ToLower() == "stop 1")
                        {
                            StopMusic();
                        }
                    }
                    else
                    {
                        //this.Close();
                        if (e.Result.Text.ToLower() == "kill your self")
                        {
                            ReadText("Self kill activated , Are you ready, closing program in 5 4 3 2 1 second , Closed");
                            this.Close();
                        }
                        if (e.Result.Text.ToLower() == "accident")
                        {
                            var x = CoreTranslation(e.Result.Text);
                            var checkdata = _context.TranslationWords.Where(c => c.EnglishWord.Equals(x));
                            if (checkdata != null)
                            {
                                TranslationWords translation = new TranslationWords();
                                translation.EnglishWord = e.Result.Text;
                                //cal function here 
                                translation.French = x;
                                _context.TranslationWords.Add(translation);
                                _context.SaveChanges();
                                _context.Dispose();
                            }
                            else
                            {
                                richTextBox1.AppendText($"Word {e.Result.Text} Already exists to database{Environment.NewLine}");
                            }
                        }

                        if (e.Result.Text.ToLower() == "cleaning")
                        {
                            await CleanUpPrefetch(path);

                            richTextBox1.AppendText("Cleaning dine Done");
                            ReadText("Pc Cleaning Operation Done , Stand By");
                        }
                        if (e.Result.Text.ToLower() == "pedarsag")
                        {
                            var cpuTemperature = await GetCpuTemperature();
                            var x = cpuTemperature.ToString();
                            richTextBox1.AppendText($"Tempreture is {GetCpuTemperature().Result}{Environment.NewLine}");
                        }
                        if (e.Result.Text.ToLower().Contains("shadow") || e.Result.Text.ToLower().Contains("stranger")
                        || e.Result.Text.ToLower().Contains("guest"))
                        {
                            string myFinalSpeech;
                            List<string>? MyIntroduceSentencess = new List<string>();
                            MyIntroduceSentencess.Add("Hey stranger , how yu become inside  , are you really stranger");
                            MyIntroduceSentencess.Add("hello guest welcome here , let me introduce me im shadow nicec to meet you");
                            MyIntroduceSentencess.Add("I Am shadow welcome here ");
                            if (e.Result.Text.Contains("shadow"))
                            {
                                myFinalSpeech = MyIntroduceSentencess.ElementAt(2);
                                ReadText(myFinalSpeech);
                            }
                            if (e.Result.Text.Contains("stranger"))
                            {
                                myFinalSpeech = MyIntroduceSentencess.ElementAt(0);
                                ReadText(myFinalSpeech);
                            }
                            if (e.Result.Text.Contains("guest"))
                            {
                                myFinalSpeech = MyIntroduceSentencess.ElementAt(1);
                                ReadText(myFinalSpeech);
                            }
                        }
                        if (e.Result.Text.ToLower().Contains("i am very sad today") || e.Result.Text.ToLower().Contains("number 40")
                        || e.Result.Text.ToLower().Contains("i am sleepy") || e.Result.Text.ToLower().Contains("guest"))
                        {
                            var x = await MoreDynamic(e.Result.Text);
                            ReadText(x);
                        }
                        if (e.Result.Text.ToLower() == "i am very fine today")
                        {
                            string? firstSentenceBAdMOod = null;
                            string firstSentenceNiceMood;
                            List<string>? MyIntroduceSentencess = new List<string>();
                            MyIntroduceSentencess.Add("Oh very nice that you are happy today i hope you enjoy the day");
                            //  firstSentenceBAdMOod = MyIntroduceSentencess.FirstOrDefault(sentence => e.Result.Text.Contains("very sad"));
                            firstSentenceNiceMood = MyIntroduceSentencess.FirstOrDefault(sentence => e.Result.Text.Contains("very fine"));
                            ReadText(MyIntroduceSentencess.ElementAt(0));
                        }

                        if (e.Result.Text.ToLower() == "my name is armin nice to meet you")
                        {
                            ReadText("oh really , i appreciate it , But you are you really readu for this");
                        }
                        if (e.Result.Text.ToLower() == "i am goin to walk")
                        {
                            var x = await GetWeatherData("Southampton");
                            richTextBox1.AppendText(x);

                            ReadText($"This is very nice try to get positive wave, i am stayin here , please attention this is wether information of the day  {x} , and happy walking ,im going to record times you walking to see how it goes each month ");
                        }
                        if (e.Result.Text.ToLower() == "winter")
                        {
                            ReadText("Translated word in french is je suis en train de manger");
                            TranslateSingelWordToFrench("hiver");
                        }
                        if (e.Result.Text.ToLower() == "i am back")
                        {
                            // ReadText("welcome back please provide your code , at the mean time make thee or coffe or what you want");
                            var xx = await GetBinancePrice("BTCUSDT");
                            var text = xx;
                            ReadText($"welcome back sir today is {DateTime.Now} , if you want deep information provide 4 digit code otherwise{text} ");

                            // TranslateSingelWordToFrench(" Bienvenue monsieur");
                            //  TranslateSingleWordToSpeechAsync("Bienvenue");
                        }
                        if (e.Result.Text.ToLower() == "number 1")
                        {
                            var finalSpeech = await GetBinancePrice("BTCUSDT");
                            var xx = finalSpeech.ToDouble();
                            var lllast = Math.Round(xx);
                            richTextBox1.AppendText($" BTC price : {lllast}{Environment.NewLine}");
                            ReadText($"Bitcoin Price is {lllast.ToString()}");
                        }
                        if (e.Result.Text.ToLower() == "richbox")
                        {
                            richTextBox1.Clear();
                            ReadText("Information logger is cleared stay standby");
                        }
                        if (e.Result.Text.ToLower() == "100")
                        {
                            var Finalmoving = await CalculateMovingAverage(apiUrl);
                            var xx = Finalmoving;
                            var lllast = Math.Round(xx);
                            richTextBox1.AppendText($" Movig Avegarge 100 Days : {lllast}{Environment.NewLine}");
                            ReadText($"Moving Average 100 days is {lllast.ToString()}");
                        }
                        if (e.Result.Text.ToLower() == "number 2")
                        {
                            double PriceHelper;

                            var finalSpeech = await GetBinancePrice("FTMUSDT");
                            var xx = finalSpeech.ToDouble();
                            richTextBox1.AppendText($" Fantom price is : {xx}{Environment.NewLine}");

                            CoinAnalysis newData = new CoinAnalysis();
                            newData.Date = DateTime.Now;
                            newData.CoinName = "FTM";
                            newData.Price = xx;
                            newData.MovingAverage100 = 0.2614;
                            PriceHelper = xx;
                            _context.CoinAnalysis.Add(newData);
                            _context.SaveChanges();
                            _context.Dispose();
                            ReadText($"Fantom Price is {xx}");

                            var Conn = new ApplicationDbContext();
                            var CkeckOlderData = Conn.CoinAnalysis.ToList();
                            var OneHoUREeARLIER = DateTime.Now.AddHours(-1).Hour;

                            var MyCOmpektedata = CkeckOlderData
                                 .Where(c => c.Date?.Hour == OneHoUREeARLIER)
                                 .Select(c => c.Price);

                            double oldprice = (double)MyCOmpektedata.FirstOrDefault();
                            var xxc = (xx - oldprice) * 100;
                            double percentageChange = ((xx - oldprice) / Math.Abs(oldprice)) * 100;
                            richTextBox1.AppendText($"Precentagr Change %{percentageChange.ToString("F3")}{Environment.NewLine}");
                            ReadText($"Compair To 1 Hour Past is ,{percentageChange.ToString("F3")}");
                            Conn.Dispose();
                        }
                        if (e.Result.Text.ToLower() == "what is news")
                        {
                            var CheckOrNewsExistsInDb = _context.News.Where(c => c.MyNews.Contains(richTextBox1.Text)).FirstOrDefault();
                            if (CheckOrNewsExistsInDb != null)
                            {
                                var datefromdb = CheckOrNewsExistsInDb.CurrentDate;
                                ReadText($"Big Dadee, This News Is Already Exists in our Database. I have read this news at {datefromdb}, I am not going to read it anymore. But if you want to rehear it, let me know. You know how to reach me?");
                            }
                            else
                            {
                                string url = "https://www.forbes.com/sites/digital-assets/";
                                List<string> MyList = await GetH3Headers(url);
                                foreach (var x in MyList)
                                {
                                    richTextBox1.AppendText($"{x}{Environment.NewLine}");
                                }
                                News MyNews1 = new News();
                                MyNews1.MyNews = richTextBox1.Text;
                                var HelperCOntext = new ApplicationDbContext();
                                HelperCOntext.News.Add(MyNews1);
                                HelperCOntext.SaveChanges();
                                HelperCOntext.Dispose();
                                ReadText($"{richTextBox1.Text}");
                            }
                        }

                        if (e.Result.Text.ToLower() == "crypto daily")
                        {

                            List<string> headingTags = await GetAllHeadingTags(Url);
                            foreach (var c in headingTags)
                            {

                                richTextBox1.AppendText($"{c}{Environment.NewLine}");
                            }

                            ReadText(richTextBox1.Text);
                        }

                        if (e.Result.Text.ToLower() == "you are my lovely client")
                        {
                            ReadText($"Thank you sir, You are welcome , maybe i will love you later,  kiss kiss , do you need code to remember you next time");
                        }
                        if (e.Result.Text.ToLower() == "thank you")
                        {

                            ReadText($"you are welcome");
                        }
                        if (e.Result.Text.ToLower() == "number 6")
                        {


                            // string symbol = "BTC-USDT"; // Replace with the trading pair you want to trade

                            //decimal notional = "11.10";


                            // decimal price = decimal.Parse(11.0); // Replace with the desired limit buy price
                            string symbol = "BTC-USDT"; // Replace with the trading pair you want to trade
                            decimal quantity = 12m; // Replace with the quantity you want to buy

                            var response = await _okexApiClient.PlaceMarketBuyOrder(symbol, quantity);

                            richTextBox1.AppendText(response);
                            ReadText($"Order Succesd , Completed");
                        }
                        else
                        {
                            await PlaySong(e.Result.Text);
                        }
                    }

                }
                else
                {

                }
            };
        }
        private async Task<List<string>> GetAllHeadingTags(string url)
        {
            List<string> headingTags = new List<string>();

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Send the HTTP GET request and get the response
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the content as a string
                        string htmlContent = await response.Content.ReadAsStringAsync();

                        // Parse the HTML content using HtmlAgilityPack
                        var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(htmlContent);

                        // Select all heading tags (h1, h2, h3, h4, h5, h6)
                        var headingNodes = htmlDocument.DocumentNode.SelectNodes("//h1 | //h2 | //h3 | //h4 | //h5 | //h6");
                        if (headingNodes != null)
                        {
                            foreach (var headingNode in headingNodes)
                            {
                                string headingText = headingNode.InnerText.Trim();
                                headingTags.Add(headingText);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to fetch the page. Status code: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return headingTags;
        }
        public static void ReadText(string text)
        {
            using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
            {
                // Set the default system voice
                synthesizer.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.Teen, 2);
                // synthesizer.SelectVoice("C:\\Users\\Armin\\Downloads\\sami.mp3");
                // Speak the provided text
                synthesizer.Speak(text);
                synthesizer.Dispose();
            }
        }

        public void TranslateSingelWordToFrench(string IncomingWord)
        {
            using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
            {

                synthesizer.SetOutputToDefaultAudioDevice();
                synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0, new System.Globalization.CultureInfo("es-ES"));
                SpeakText(synthesizer, IncomingWord);
                synthesizer.Dispose();
            }
        }
        static void SpeakText(SpeechSynthesizer synthesizer, string text)
        {
            synthesizer.Speak(text);
        }
        private string RemoveHtmlTags(string html)
        {
            return Regex.Replace(html, "<.*?>", string.Empty);
        }

        public async Task PlaySong(string option)
        {
            switch (option)
            {
                case "Tupac":
                    await PlayMusicAsync($"C:\\Users\\ArsaM\\Desktop\\AudioDownloaded\\Tupac.mp3{Environment.NewLine}");
                    break;
                case "Mehrdad":
                    await PlayMusicAsync("C:\\Users\\ArsaM\\Desktop\\AudioDownloaded\\MehrdadHidden-Nakhla.mp3");
                    break;
                case "Arsam":
                    await PlayMusicAsync("C:\\Users\\ArsaM\\Desktop\\AudioDownloaded\\balance.mp3");
                    break;
                case "Vigen":
                    await PlayMusicAsync("C:\\Users\\ArsaM\\Desktop\\AudioDownloaded\\Vigen-Hayde-Hamkhine.mp3");
                    break;
                case "Chavoshi":
                    await PlayMusicAsync("C:\\Users\\ArsaM\\Desktop\\AudioDownloaded\\chavoshi-mtasel.mp3");
                    break;
                case "247":
                    await PlayMusicAsync("C:\\Users\\ArsaM\\Desktop\\AudioDownloaded\\247HosseinEBLIS.MP3");
                    break;
                case "number fifty":
                    ReadText($"Music By :  Amir Tattaloo Song name: Allah, ");
                    await PlayMusicAsync("C:\\Users\\ArsaM\\Desktop\\AudioDownloaded\\Tataloo-Allah.mp3");
                    break;
                case "goriz":
                    ReadText($"Music By :  Ebi  Song name: Goreez, ");
                    await PlayMusicAsync("C:\\Users\\ArsaM\\Desktop\\AudioDownloaded\\Ebi-Goriz.mp3");
                    break;
                case "naaz":
                    ReadText($"Music by  : Moshsen Chaavoshi.   Song name: Naaz, ");
                    await PlayMusicAsync("C:\\Users\\ArsaM\\Desktop\\AudioDownloaded\\chavoshi-naz.mp3");
                    break;
                case "number 4":
                    await PlayMusicAsync("C:\\Users\\ArsaM\\Desktop\\AudioDownloaded\\Pochil.mp3");
                    break;
                default:
                    // Console.WriteLine("Invalid option. Please select between 1, 2, or 3.");
                    //  richTextBox1.AppendText($"InValid Option{label16.Text}");
                    // ReadText("French is hiver");
                    // Thread.Sleep(2000);
                    //  ReadText("Spanish  is invierno");
                    break;
            }
        }
        public async Task PlayMusicAsync(string filePath)
        {
            try
            {
                using (var audioFileReader = new AudioFileReader(filePath))
                {
                    waveOutEvent = new WaveOutEvent();
                    waveOutEvent.Init(audioFileReader);
                    waveOutEvent.Play();

                    isPlaying = true; // Music started playing
                    richTextBox1.AppendText($"Playing Musick {filePath}{Environment.NewLine}");
                    while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                    {
                        // Wait until the music finishes playing
                        await Task.Delay(100);
                    }

                    //Console.WriteLine("Music playback finished.");
                    richTextBox1.AppendText($"Musick Paly Back Finished{Environment.NewLine}");
                    isPlaying = false; // Music stopped playing

                    waveOutEvent.Stop();
                    waveOutEvent.Dispose();
                }
            }
            catch (Exception ex)
            {
                //  Console.WriteLine("An error occurred while playing the music: " + ex.Message);
            }
        }
        private const string ApiKey2 = "4af73d31c590a47216010f82f1a92878";
        private const string BaseUrl2 = "http://api.openweathermap.org/data/2.5/weather";

        public async Task<string> GetWeatherData(string city)
        {
            string apiUrl = $"{BaseUrl2}?q={city}&appid={ApiKey2}&units=metric";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                        string main = data.weather[0].main;
                        string description = data.weather[0].description;
                        double temp = data.main.temp;
                        double temp_min = data.main.temp_min;

                        return $"Main: {main}, Description: {description}, Temp: {temp}, Temp_min: {temp_min}";
                    }
                    else
                    {
                        // Handle the case where the API request was not successful
                        return "Unable to fetch weather data.";
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exception that occurred during the API request
                    return $"An error occurred: {ex.Message}";
                }
            }
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            string apiUrl = "https://api.coingecko.com/api/v3/coins/bitcoin/market_chart?vs_currency=usd&days=100";
            int timeIntervalInMinutes = 15;
            decimal movingAverage = await CalculateMovingAverage(apiUrl, timeIntervalInMinutes);

            richTextBox1.AppendText(movingAverage.ToString());
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            var MyData = _context.Information
                .Where(c => c.InformationID == 2);
            foreach (var x in MyData)
            {
                textBox3.Text = x.InformationID.ToString();
                textBox4.Text = x.ShutDownCount.ToString();
                textBox5.Text = x.RestartCount.ToString();

            }
            richTextBox1.AppendText("Operatio  nDone");
            _context.Dispose();

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            var MyData = new Information();
            MyData.RestartCount = textBox4.Text;
            MyData.ShutDownCount = textBox5.Text;
            _context.Information.Add(MyData);
            _context.SaveChanges();
            _context.Dispose();
        }

        public void StopMusic()
        {
            waveOutEvent?.Stop();
            waveOutEvent?.Dispose();
            richTextBox1.AppendText($"Stoped Stoped{Environment.NewLine}");
            isPlaying = false;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            recognizer.Dispose();
            waveOutEvent.Dispose();
            _context.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {

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
                        richTextBox1.Text = file;
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
        public async Task<int> CleanUpPrefetch(string path)
        {
            try
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    try
                    {
                        File.Delete(file);
                        richTextBox1.AppendText(file);
                    }
                    catch (IOException Ex)
                    {
                        richTextBox1.AppendText($"{Ex.Message}");
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

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public class CoinGeckoResponse
    {
        [JsonProperty("prices")]
        public decimal[][] Prices { get; set; }
    }
}