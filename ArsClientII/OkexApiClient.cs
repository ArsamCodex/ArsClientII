using System.Security.Cryptography;
using System.Text;

public class OkexApiClient
{
    private const string ApiKey = "fcf7293a-dc47-4b54-8ceb-deaedbb7f7d7";
    private const string SecretKey = "6364844206A41E7F389C365C9480211E";
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
        request.Headers.Add("OK-ACCESS-PASSPHRASE", "Arsam-x5252"); // Replace with your passphrase
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
}