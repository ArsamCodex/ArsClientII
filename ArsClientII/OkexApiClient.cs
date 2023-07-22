using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

public class OkexApiClient
{
    private const string ApiKey = "";
    private const string SecretKey = "";
    private const string BaseUrl = "https://www.okex.com";

    private readonly HttpClient _httpClient;
    private readonly int _timestampPaddingSeconds = 10;

    private string _timestamp;

    public OkexApiClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };

        // Generate timestamp once during the constructor call
        _timestamp = DateTimeOffset.UtcNow.AddSeconds(_timestampPaddingSeconds).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
    }

    private string GenerateSignature(string method, string requestPath, string body = "")
    {
        var prehashString = $"{_timestamp}{method.ToUpper()}{requestPath}{body}";
        var secretBytes = Encoding.UTF8.GetBytes(SecretKey);
        using (var hmac = new HMACSHA256(secretBytes))
        {
            var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(prehashString));
            return Convert.ToBase64String(signatureBytes);
        }
    }


    public HttpRequestMessage CreateRequest(string method, string requestPath, string body = "")
    {
        var signature = GenerateSignature(method, requestPath, body);

        var request = new HttpRequestMessage(new HttpMethod(method), requestPath);
        request.Headers.Add("OK-ACCESS-KEY", ApiKey);
        request.Headers.Add("OK-ACCESS-SIGN", signature);
        request.Headers.Add("OK-ACCESS-TIMESTAMP", _timestamp);
        request.Headers.Add("OK-ACCESS-PASSPHRASE", "Papa-557"); // Replace with your passphrase
        request.Content = new StringContent(body, Encoding.UTF8, "application/json");

        return request;
    }


    public async Task<string> PlaceMarketBuyOrder(string symbol, decimal quantity)
    {
        var endpoint = "/api/v5/trade/order";
        var requestBody = $"{{ \"instId\": \"{symbol}\", \"tdMode\": \"cash\", \"side\": \"buy\", \"ordType\": \"market\", \"sz\": \"{quantity}\" }}";

        var request = CreateRequest("POST", endpoint, requestBody);

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        return responseContent;
    }


    public async Task<decimal> GetAssetsAsync()
    {

        // Replace with the actual endpoint for getting account balances.
        var endpoint = "/api/v5/account/balance";

        // Add any required headers (e.g., authorization).
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-KEY", ApiKey);
        _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-SIGN", GenerateSignature("GET", endpoint)); // Use "GET" method for balance retrieval.
        _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-TIMESTAMP", _timestamp);
        _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-PASSPHRASE","");

        var response = await _httpClient.GetAsync($"{BaseUrl}{endpoint}");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var c = ExtractBalance(responseContent, "BTC");

        return c;

    }
    private decimal ExtractBalance(string responseContent, string currency)
    {
        var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);

        if (jsonResponse.TryGetValue("data", out var data))
        {
            var dataArray = (data as Newtonsoft.Json.Linq.JArray).ToObject<List<Dictionary<string, object>>>();
            foreach (var entry in dataArray)
            {
                if (entry.TryGetValue("details", out var details))
                {
                    var detailsArray = (details as Newtonsoft.Json.Linq.JArray).ToObject<List<Dictionary<string, object>>>();
                    foreach (var currencyEntry in detailsArray)
                    {
                        if (currencyEntry.TryGetValue("ccy", out var ccy) && currencyEntry.TryGetValue("availBal", out var availBal))
                        {
                            if (ccy.ToString() == currency)
                            {
                                return decimal.Parse(availBal.ToString());
                            }
                        }
                    }
                }
            }
        }

        // Return 0 if the currency is not found in the response.
        return 0;
    }
    /*
    public async Task<List<OkexAsset>> GetAssetsAsync()
    {
        try
        {
            // var endpoint = "/api/v5/trade/order";
            // var requestBody = $"{{ \"instId\": \"{symbol}\", \"tdMode\": \"cash\", \"side\": \"buy\", \"ordType\": \"market\", \"sz\": \"{quantity}\" }}";
            var endpoint = "/api/v5/account/balance";

        var response = await _httpClient.GetAsync($"{BaseUrl}{endpoint}");
        response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<OkexApiResponse>(responseContent);

            if (apiResponse.Success)
                return apiResponse.Data;
            else
                throw new Exception("OKEx API request was not successful.");
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during the API request.
            throw new Exception("Error retrieving OKEx assets: " + ex.Message);
        }





    }
    */
    public class OkexAsset
    {
        public string Currency { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal AvailableBalance { get; set; }
    }
    public class OkexApiResponse
    {
        public bool Success { get; set; }
        public List<OkexAsset> Data { get; set; }
    }
}