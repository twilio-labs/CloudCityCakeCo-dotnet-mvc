using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Lookups.V1;
using Twilio.Types;
using CloudCityCakeCo.Models.DTO;
using CloudCityCakeCo.Models.Helpers;
using CloudCityCakeCo.Services.Interfaces;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudCityCakeCo.Services.Implementations
{
    public class PhoneNumberService : IPhoneNumberService
    {
        private readonly TwilioAccount _account;
        private readonly IHttpClientFactory _clientFactory;
        public PhoneNumberService(
            IOptions<TwilioAccount> account,
            IHttpClientFactory clientFactory)
        {
            _account = account.Value ?? throw new ArgumentNullException(nameof(account));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            TwilioClient.Init(_account.AccountSid, _account.AuthToken);
        }


        public async Task<ServiceResponse<PhoneNumberTransferResource>> LookupNumber(string phoneNumber)
        {

            if (phoneNumber.StartsWith("whatsapp:"))
            {
                var splitResult = phoneNumber.Split(":");
                phoneNumber = splitResult[1];
            }
            PhoneNumberResource number = null;

            number = await
                  PhoneNumberResource.FetchAsync(
                      pathPhoneNumber: new PhoneNumber(phoneNumber));

            var code = await GetCountryCode(number.CountryCode);
           
            Console.WriteLine(number.NationalFormat);

            var phoneNumberTransferResource = new PhoneNumberTransferResource
            {
                PhoneNumber = number.PhoneNumber.ToString(),
                CountryCode = number.CountryCode,
                CountryCodePrefix = $"+{code}",
                NationalFormat = number.NationalFormat
            };
            var serviceResponse = new ServiceResponse<PhoneNumberTransferResource>
            {
                Content = phoneNumberTransferResource,
                Message = "Nationally formatted number"
            };

            return serviceResponse;

        }

        private async Task<string> GetCountryCode(string countryCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
           "http://country.io/phone.json");
         

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
                //JsonSerializer.Deserialize<Dictionary<string,string>>(responseBody)
            }
            else
            {
            }

            dictionary.TryGetValue(countryCode, out var code);

            return code;

        }

    }
}
