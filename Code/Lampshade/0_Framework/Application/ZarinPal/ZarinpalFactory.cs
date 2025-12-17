using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace _0_Framework.Application.ZarinPal
{
    public class ZarinPalFactory : IZarinPalFactory
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public string Prefix { get; set; }
        private string MerchantId { get; }

        public ZarinPalFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            Prefix = _configuration.GetSection("payment")["method"];
            MerchantId = _configuration.GetSection("payment")["merchant"];
            _httpClient = new HttpClient();
        }

        public PaymentResponse CreatePaymentRequest(string amount, string mobile, string email, string description,
             long orderId)
        {
            amount = amount.Replace(",", "");
            var finalAmount = int.Parse(amount);
            var siteUrl = _configuration.GetSection("payment")["siteUrl"];

            var baseUrl = Prefix == "sandbox"
                ? "https://sandbox.zarinpal.com/pg/v4/payment/request.json"
                : "https://api.zarinpal.com/pg/v4/payment/request.json";

            object body;
            if (!string.IsNullOrEmpty(mobile) || !string.IsNullOrEmpty(email))
            {
                body = new
                {
                    merchant_id = MerchantId,
                    amount = finalAmount,
                    callback_url = $"{siteUrl}/Checkout?handler=CallBack&oId={orderId}",
                    description = description,
                    metadata = new { mobile = mobile ?? "", email = email ?? "" }
                };
            }
            else
            {
                body = new
                {
                    merchant_id = MerchantId,
                    amount = finalAmount,
                    callback_url = $"{siteUrl}/Checkout?handler=CallBack&oId={orderId}",
                    description = description
                };
            }

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = _httpClient.PostAsync(baseUrl, content).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            
            // Debug log
            System.Diagnostics.Debug.WriteLine($"ZarinPal Request: {json}");
            System.Diagnostics.Debug.WriteLine($"ZarinPal Response: {responseContent}");
            Console.WriteLine($"ZarinPal Request: {json}");
            Console.WriteLine($"ZarinPal Response: {responseContent}");

            var result = new PaymentResponse();
            if (!string.IsNullOrEmpty(responseContent))
            {
                // Check if response is JSON (starts with {)
                if (!responseContent.TrimStart().StartsWith("{"))
                {
                    Console.WriteLine($"ZarinPal returned non-JSON response (possibly blocked or error page)");
                    result.Status = -999;
                    return result;
                }
                
                try
                {
                    using var doc = JsonDocument.Parse(responseContent);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("data", out var data) && data.ValueKind != JsonValueKind.Array)
                    {
                        if (data.TryGetProperty("authority", out var authority))
                            result.Authority = authority.GetString();
                        if (data.TryGetProperty("code", out var code))
                            result.Status = code.GetInt32();
                    }
                    else if (root.TryGetProperty("errors", out var errors) && errors.ValueKind != JsonValueKind.Array)
                    {
                        if (errors.TryGetProperty("code", out var errorCode))
                            result.Status = errorCode.GetInt32();
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"JSON Parse Error: {ex.Message}");
                    result.Status = -998;
                }
            }

            return result;
        }

        public VerificationResponse CreateVerificationRequest(string authority, string amount)
        {
            amount = amount.Replace(",", "");
            var finalAmount = int.Parse(amount);

            var baseUrl = Prefix == "sandbox"
                ? "https://sandbox.zarinpal.com/pg/v4/payment/verify.json"
                : "https://api.zarinpal.com/pg/v4/payment/verify.json";

            var body = new
            {
                merchant_id = MerchantId,
                amount = finalAmount,
                authority = authority
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = _httpClient.PostAsync(baseUrl, content).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            var result = new VerificationResponse();
            if (!string.IsNullOrEmpty(responseContent))
            {
                using var doc = JsonDocument.Parse(responseContent);
                var root = doc.RootElement;

                if (root.TryGetProperty("data", out var data) && data.ValueKind != JsonValueKind.Array)
                {
                    if (data.TryGetProperty("code", out var code))
                        result.Status = code.GetInt32();
                    if (data.TryGetProperty("ref_id", out var refId))
                        result.RefID = refId.GetInt64();
                }
                else if (root.TryGetProperty("errors", out var errors) && errors.ValueKind != JsonValueKind.Array)
                {
                    if (errors.TryGetProperty("code", out var errorCode))
                        result.Status = errorCode.GetInt32();
                }
            }

            return result;
        }
    }
}
