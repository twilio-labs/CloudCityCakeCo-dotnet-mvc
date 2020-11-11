using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CloudCityCakeCo.Models.DTO;
using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Services.Interfaces;
using CloudCityCakeCo.Models.Helpers;

using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace CloudCityCakeCo.Services.Implementations
{
    public class VerifyService : IVerifyService
    {
        private readonly VerifySettings _verifySettings;
        private readonly ILogger<VerifyService> _logger;

        public VerifyService(IOptions<VerifySettings> optionsAccessor,
            IHttpClientFactory clientFactory,
            ILogger<VerifyService> logger)
        {
            string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");

            TwilioClient.Init(accountSid, authToken);

            string verifyServiceSid = Environment.GetEnvironmentVariable("VERIFY_SERVICE_SID");
        }


        public async Task<string> SendSmsAsync(string PhoneNumber, string verifyServiceSid)
        {
            var verification = VerificationResource.Create(
                to: PhoneNumber,
                channel: "sms",
                pathServiceSid: verifyServiceSid
            );

            return verification;
        }

        public async Task<TokenVerificationResult> VerifyPhoneTokenAsync(string PhoneNumber, string token, string verifyServiceSid)
        {

            var verificationCheck = VerificationCheckResource.Create(
                to: PhoneNumber,
                code: token,
                pathServiceSid: verifyServiceSid
            );

            Console.WriteLine(verificationCheck.Status);

            if (verificationCheck.Status == "approved")
            {
                return new TokenVerificationResult("ok");
            }

            return new TokenVerificationResult("incorrect token", false);
        }


        
    }
}