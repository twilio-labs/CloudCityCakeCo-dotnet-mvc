using System;
using System.Threading.Tasks;
using CloudCityCakeCo.Models.DTO;
using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Models.Enums;
using CloudCityCakeCo.Models.Helpers;
using CloudCityCakeCo.Services.Interfaces;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace CloudCityCakeCo.Services.Implementations
{
    public class MessagingService : IMessagingService
    {
        private readonly TwilioAccount _account;

        public MessagingService(IOptions<TwilioAccount> account)
        {
            _account = account.Value ?? throw new ArgumentNullException();
        }

        public async Task<ServiceResponse> SendMessage(CakeOrder cakeOrder)
        {
            TwilioClient.Init(_account.AccountSid, _account.AuthToken);

            var message = await MessageResource
                .CreateAsync(from: new PhoneNumber("whatsapp:+14155238886"),
                    to: new PhoneNumber(cakeOrder.User.Number),
                    body: $"Your cake order's status code is {cakeOrder.OrderStatus.ToString()}.");

            return new ServiceResponse
            {
                Message = "WhatsApp message was sent",
                ServiceResponseStatus = ServiceResponseStatus.Ok
            };
        }
    }
}