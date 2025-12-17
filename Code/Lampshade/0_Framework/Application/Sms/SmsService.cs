using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SmsIrRestful;

namespace _0_Framework.Application.Sms
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string number, string message)
        {
            try
            {
                var token = GetToken();
                if (string.IsNullOrEmpty(token)) return;
                
                var lines = new SmsLine().GetSmsLines(token);
                if (lines == null || lines.SMSLines == null || !lines.SMSLines.Any()) return;

                var line = lines.SMSLines.Last().LineNumber.ToString();
            var data = new MessageSendObject
            {
                Messages = new List<string>
                    {message}.ToArray(),
                MobileNumbers = new List<string> {number}.ToArray(),
                LineNumber = line,
                SendDateTime = DateTime.Now,
                CanContinueInCaseOfError = true
            };
            var messageSendResponseObject = 
                new MessageSend().Send(token, data);

                if (messageSendResponseObject.IsSuccessful) return;

                line = lines.SMSLines.First().LineNumber.ToString();
                data.LineNumber = line;
                new MessageSend().Send(token, data);
            }
            catch (Exception)
            {
                // SMS service not configured, skip silently
            }
        }

        private string GetToken()
        {
            var smsSecrets = _configuration.GetSection("SmsSecrets");
            var tokenService = new Token();
            return tokenService.GetToken(smsSecrets["ApiKey"], smsSecrets["SecretKey"]);
        }
    }
}