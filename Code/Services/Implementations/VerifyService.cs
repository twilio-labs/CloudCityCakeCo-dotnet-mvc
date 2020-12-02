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
using CloudCityCakeCo.Models.Enums;

namespace CloudCityCakeCo.Services.Implementations
{
    public class VerifyService : IVerifyService
    {
        private readonly VerifySettings _verifySettings;
        private readonly TwilioAccount _twilioAccount;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;
        private readonly ILogger<VerifyService> _logger;

        public VerifyService(IOptions<VerifySettings> verifySettings,
            IOptions<TwilioAccount> twilioAccount,
            IHttpClientFactory clientFactory,
            ILogger<VerifyService> logger)
        {
            _verifySettings = verifySettings.Value;
            _twilioAccount = twilioAccount.Value;
            _logger = logger;

            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient();

            TwilioClient.Init(_twilioAccount.AccountSid, _twilioAccount.AuthToken);



        }


        public async Task SendSmsAsync(string phoneNumber)
        {
            var serviceResponse = new ServiceResponse
            {
                ServiceResponseStatus = ServiceResponseStatus.Unset
            };

            var verification = await VerificationResource.CreateAsync(
               to: phoneNumber,
               channel: "sms",
               pathServiceSid: _verifySettings.VerifyServiceId);

            _logger.LogDebug("VerificationSid: " + verification.Sid.ToString());

        }

        public async Task<ServiceResponse> VerifyPhoneTokenAsync(string phoneNumber, string token)
        {
            
            var serviceResponse = new ServiceResponse
            {
                ServiceResponseStatus = ServiceResponseStatus.Unset
            };

            var verificationCheck = await VerificationCheckResource.CreateAsync(
                to: phoneNumber,
                code: token,
                pathServiceSid: _verifySettings.VerifyServiceId
            );


            if (verificationCheck.Status == "approved")
            {
                serviceResponse.ServiceResponseStatus = ServiceResponseStatus.Ok;
               
            }
            return serviceResponse;
        }

    }
}