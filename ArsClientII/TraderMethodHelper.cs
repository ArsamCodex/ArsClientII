using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArsClientII
{
    public class TraderMethodHelper
    {
        string apiUrl = "https://api.coingecko.com/api/v3/coins/bitcoin/market_chart?vs_currency=usd&days=100";
        string apiUrl200 = "https://api.coingecko.com/api/v3/coins/bitcoin/market_chart?vs_currency=usd&days=200";


        static async Task<decimal> CalculateMovingAverage(string apiUrl0, int timeIntervalInMinutes)
        {
            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync(apiUrl0);
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

        public async Task<decimal> Get100Daily()
        {
            int timeIntervalInMinutes = 1440;
            decimal movingAverageDaily = await CalculateMovingAverage(apiUrl, timeIntervalInMinutes);
           // richTextBox1.AppendText($" Moving Average 100 Time Frame 15 Minutes {movingAverageDaily.ToString("F3")} {Environment.NewLine}");
            return movingAverageDaily;
        }
        public async Task<decimal> Get10015Min()
        {
            int timeIntervalInMinutes = 15;
            decimal movingAverage = await CalculateMovingAverage(apiUrl, timeIntervalInMinutes);
           // richTextBox1.AppendText($" Moving Average 100 Time Frame 15 Minutes {movingAverage.ToString("F3")} {Environment.NewLine}");
            return movingAverage;
        }
        public async Task<decimal> Get1005Min()
        {
            int timeIntervalInMinutes5 = 5;
            decimal movingAverage5 = await CalculateMovingAverage(apiUrl, timeIntervalInMinutes5);
          //  richTextBox1.AppendText($" Moving Average 100 Time Frame 5 Minutes {movingAverage5.ToString("F3")} {Environment.NewLine}");
            return movingAverage5;
        }
        public async Task<decimal> Get10060Min()
        {
            int timeIntervalInMinutes60 = 60;
            decimal movingAverage60 = await CalculateMovingAverage(apiUrl, timeIntervalInMinutes60);
            // richTextBox1.AppendText($" Moving Average 100 Time Frame 5 Minutes {movingAverage60.ToString("F3")} {Environment.NewLine}");
            return movingAverage60;
        }
        public async Task<decimal> Get200Daily()
        {
            int timeIntervalInMinutes = 1440;
            decimal movingAverageDaily200Daily = await CalculateMovingAverage(apiUrl200, timeIntervalInMinutes);
            // richTextBox1.AppendText($" Moving Average 100 Time Frame 15 Minutes {movingAverageDaily200Daily.ToString("F3")} {Environment.NewLine}");
            return movingAverageDaily200Daily;
        }
        public async Task<decimal> Get200T60()
        {
            int timeIntervalInMinutes = 60;
            decimal movingAverageDaily200T60 = await CalculateMovingAverage(apiUrl200, timeIntervalInMinutes);
            // richTextBox1.AppendText($" Moving Average 100 Time Frame 15 Minutes {movingAverageDaily200Daily.ToString("F3")} {Environment.NewLine}");
            return movingAverageDaily200T60;
        }
        public async Task<decimal> Get200T15()
        {
            int timeIntervalInMinutes = 15;
            decimal movingAverageDaily200T15 = await CalculateMovingAverage(apiUrl200, timeIntervalInMinutes);
            // richTextBox1.AppendText($" Moving Average 100 Time Frame 15 Minutes {movingAverageDaily200Daily.ToString("F3")} {Environment.NewLine}");
            return movingAverageDaily200T15;
        }
        public async Task<decimal> Get200T5()
        {
            int timeIntervalInMinutes = 5;
            decimal movingAverageDaily200T5 = await CalculateMovingAverage(apiUrl200, timeIntervalInMinutes);
            // richTextBox1.AppendText($" Moving Average 100 Time Frame 15 Minutes {movingAverageDaily200Daily.ToString("F3")} {Environment.NewLine}");
            return movingAverageDaily200T5;
        }
    }
}
