using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace _0_Framework.Application.Sms
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public SmsService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public void Send(string number, string message)
        {
            try
            {
                SendAsync(number, message).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                // SMS service not configured or failed, skip silently
            }
        }

        private async Task SendAsync(string number, string message)
        {
            var smsSecrets = _configuration.GetSection("SmsSecrets");
            var apiKey = smsSecrets["ApiKey"];
            
            if (string.IsNullOrEmpty(apiKey))
                return;

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.sms.ir/v1/");
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);

            var payload = new
            {
                lineNumber = smsSecrets["LineNumber"] ?? "30007732900900",
                messageText = message,
                mobiles = new[] { number }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync("send/bulk", content);
        }
    }
}
