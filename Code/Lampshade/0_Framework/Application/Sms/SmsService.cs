using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace _0_Framework.Application.Sms
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SmsService> _logger;

        public SmsService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<SmsService> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public void Send(string number, string message)
        {
            try
            {
                SendAsync(number, message).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMS send failed to {Number}", number);
            }
        }

        private async Task SendAsync(string number, string message)
        {
            var smsSecrets = _configuration.GetSection("SmsSecrets");
            var apiKey = smsSecrets["ApiKey"];
            var mode = smsSecrets["Mode"]?.ToLower() ?? "production";
            
            // حالت test - فقط لاگ می‌زنه، هیچ درخواستی نمیره
            if (mode == "test")
            {
                _logger.LogInformation("📱 [TEST MODE] SMS would be sent to {Number}: {Message}", number, message);
                return;
            }
            
            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogWarning("SMS ApiKey is not configured");
                return;
            }

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.sms.ir/v1/");
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            client.DefaultRequestHeaders.Add("Accept", "text/plain");

            // حالت sandbox - از verify endpoint استفاده می‌کنه (طبق مستندات sms.ir)
            if (mode == "sandbox")
            {
                await SendSandboxAsync(client, number, message);
                return;
            }

            // حالت production - ارسال واقعی
            await SendProductionAsync(client, number, message, smsSecrets["LineNumber"]);
        }

        private async Task SendSandboxAsync(HttpClient client, string number, string message)
        {
            // در Sandbox فقط verify با templateId=123456 کار می‌کنه
            var payload = new
            {
                mobile = number,
                templateId = 123456,
                parameters = new[]
                {
                    new { name = "Code", value = "12345" }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _logger.LogInformation("📱 [SANDBOX] Verify Request to {Number}", number);
            _logger.LogInformation("📱 [SANDBOX] Original message was: {Message}", message);
            
            var response = await client.PostAsync("send/verify", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
                _logger.LogInformation("📱 [SANDBOX] API call successful! Response: {Body}", responseBody);
            else
                _logger.LogWarning("📱 [SANDBOX] API call failed ({StatusCode}): {Body}", (int)response.StatusCode, responseBody);
        }

        private async Task SendProductionAsync(HttpClient client, string number, string message, string lineNumber)
        {
            if (string.IsNullOrEmpty(lineNumber))
            {
                lineNumber = await GetFirstAvailableLine(client);
                if (string.IsNullOrEmpty(lineNumber))
                {
                    _logger.LogWarning("No SMS line available");
                    return;
                }
            }
            
            var payload = new
            {
                lineNumber = lineNumber,
                messageText = message,
                mobiles = new[] { number }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _logger.LogInformation("SMS Request: {Json}", json);
            
            var response = await client.PostAsync("send/bulk", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
                _logger.LogInformation("SMS sent successfully to {Number}", number);
            else
                _logger.LogWarning("SMS Response ({StatusCode}): {Body}", (int)response.StatusCode, responseBody);
        }

        private async Task<string> GetFirstAvailableLine(HttpClient client)
        {
            try
            {
                var response = await client.GetAsync("line");
                var responseBody = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation("SMS Lines Response: {Body}", responseBody);
                
                using var doc = JsonDocument.Parse(responseBody);
                var data = doc.RootElement.GetProperty("data");
                
                if (data.ValueKind == JsonValueKind.Array && data.GetArrayLength() > 0)
                {
                    return data[0].GetInt64().ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get SMS lines");
            }
            
            return null;
        }
    }
}
