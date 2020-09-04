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

namespace CloudCityCakeCo.Services.Implementations
{
    public class AuthyService : IAuthyService
    {
        private readonly AuthySettings _authySettings;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;
        private readonly ILogger<AuthyService> _logger;

        public AuthyService(IOptions<AuthySettings> optionsAccessor,
            IHttpClientFactory clientFactory,
            ILogger<AuthyService> logger)
        {
            _authySettings = optionsAccessor.Value;
            _logger = logger;

            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://api.authy.com");
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("user-agent", "Twilio Account Security C# Sample");
            _client.DefaultRequestHeaders.Add("X-Authy-API-Key", _authySettings.AuthyId);
        }



        public async Task<string> RegisterUserAsync(User user)
        {
            var userRegData = new Dictionary<string, string>() {
                { "email", user.Email },
                { "country_code", user.CountryCode },
                { "cellphone", user.PhoneNumber }
            };
            var userRegRequestData = new Dictionary<string, object>() { };
            userRegRequestData.Add("user", userRegData);
            var encodedContent = new FormUrlEncodedContent(userRegData);


            var result = await _client
                .PostAsync("/protected/json/users/new", new StringContent(JsonConvert.SerializeObject(userRegRequestData),
                Encoding.UTF8, "application/json"));

            _logger.LogDebug(result.Content.ReadAsStringAsync().Result);

            result.EnsureSuccessStatusCode();

            var response = await result.Content.ReadAsAsync<Dictionary<string, object>>();

            return JObject.FromObject(response["user"])["id"].ToString();
        }


        public async Task<string> SendSmsAsync(string authyId)
        {
            var result = await _client.GetAsync($"/protected/json/sms/{authyId}?force=true");

            _logger.LogDebug(result.ToString());

            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsStringAsync();
        }

        public async Task<TokenVerificationResult> VerifyPhoneTokenAsync(string authyId, string token)
        {
            var result = await _client.GetAsync(
                $"protected/json/verify/{token}/{authyId}"
            );

            _logger.LogDebug(result.ToString());
            _logger.LogDebug(result.Content.ReadAsStringAsync().Result);

            var message = await result.Content.ReadAsStringAsync();

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return new TokenVerificationResult(message);
            }

            return new TokenVerificationResult(message, false);
        }


        
    }
}