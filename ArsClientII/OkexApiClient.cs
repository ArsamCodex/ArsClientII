using AngleSharp.Io;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

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

        var request = new HttpRequestMessage(new System.Net.Http.HttpMethod(method), requestPath);
        request.Headers.Add("OK-ACCESS-KEY", ApiKey);
        request.Headers.Add("OK-ACCESS-SIGN", signature);
        request.Headers.Add("OK-ACCESS-TIMESTAMP", _timestamp);
        request.Headers.Add("OK-ACCESS-PASSPHRASE", ""); // Replace with your passphrase
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
        _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-PASSPHRASE", "");

        var response = await _httpClient.GetAsync($"{BaseUrl}{endpoint}");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var c = ExtractBalance(responseContent, "BTC");

        return c;

    }
    public async Task<List<OkexAsset>> GetAllAssetsAsync()
    {
        try
        {
            var endpoint = "/api/v5/account/balance";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-KEY", ApiKey);
            _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-SIGN", GenerateSignature("GET", endpoint));
            _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-TIMESTAMP", _timestamp);
            _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-PASSPHRASE", "Papa-557");

            var response = await _httpClient.GetAsync($"{BaseUrl}{endpoint}");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            DataObject dataObject = JsonConvert.DeserializeObject<DataObject>(responseContent);

            List<OkexAsset> assets = new List<OkexAsset>();

            foreach (var detail in dataObject.Data[0].Details)
            {
                assets.Add(new OkexAsset
                {
                    Ccy = detail.Ccy,
                    AvailEq = detail.AvailEq
                });
            }

            return assets;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving OKEx assets: " + ex.Message);
        }
    }

    public static DataObject DeserializeJson(string jsonString)
    {
        return JsonConvert.DeserializeObject<DataObject>(jsonString);
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
    public async Task<decimal> GetTottalBalance()
    {
        try
        {
            var endpoint = "/api/v5/account/balance";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-KEY", ApiKey);
            _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-SIGN", GenerateSignature("GET", endpoint));
            _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-TIMESTAMP", _timestamp);
            _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-PASSPHRASE", "Papa-557");

            var response = await _httpClient.GetAsync($"{BaseUrl}{endpoint}");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            DataObject dataObject = JsonConvert.DeserializeObject<DataObject>(responseContent);

            decimal totalBalance = 0;

            foreach (var detail in dataObject.Data[0].Details)
            {
                if (decimal.TryParse(detail.AvailEq, out decimal availEq))
                {
                    totalBalance += availEq;
                }
            }

            return totalBalance;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving total balance: " + ex.Message);
        }
    }

    public class OkexAsset2
    {
        public string Currency { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal AvailableBalance { get; set; }
    }


    public class OkexAssetResponse
    {
        public string Code { get; set; }
        public List<Dictionary<string, List<OkexAsset>>> Data { get; set; }
    }

    public class OkexApiResponse
    {
        public bool Success { get; set; }
        public List<OkexAsset> Data { get; set; }
    }
    public class OkexAsset
    {
        public string Ccy { get; set; }
        public string AvailEq { get; set; }
    }
    public class DataObject
    {
        public string Code { get; set; }
        public DataItem[] Data { get; set; }
        public string Message { get; set; }
    }
    public class DataItem
    {
        public string Imr { get; set; }
        public string IsoEq { get; set; }
        public string MgnRatio { get; set; }
        public string Mmr { get; set; }
        public string NotionalUsd { get; set; }
        public string OrdFroz { get; set; }
        public string TotalEq { get; set; }
        public string UTime { get; set; }
        public Detail[] Details { get; set; }
    }
    public class Detail
    {
        public string AvailBal { get; set; }
        public string AvailEq { get; set; }
        public string CashBal { get; set; }
        public string Ccy { get; set; }
        public string CrossLiab { get; set; }
        public string DisEq { get; set; }
        public string Eq { get; set; }
        public string EqUsd { get; set; }
        public string FixedBal { get; set; }
        public string FrozenBal { get; set; }
        public string Interest { get; set; }
        public string IsoEq { get; set; }
        public string IsoLiab { get; set; }
        public string IsoUpl { get; set; }
        public string Liab { get; set; }
        public string MaxLoan { get; set; }
        public string MgnRatio { get; set; }
        public string NotionalLever { get; set; }
        public string OrdFrozen { get; set; }
        public string SpotInUseAmt { get; set; }
        public string StgyEq { get; set; }
        public string Twap { get; set; }
        public string UTime { get; set; }
        public string Upl { get; set; }
        public string UplLiab { get; set; }
    }

}
