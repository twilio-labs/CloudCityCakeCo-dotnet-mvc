using System;
using System.Threading.Tasks;
using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Models.Enums;
using CloudCityCakeCo.Models.Helpers;
using CloudCityCakeCo.Services.Interfaces;

namespace CloudCityCakeCo.Services.NotificationRules
{
    public class AcceptedNotificationRule : IStatusNotificationRule
    {
        private readonly IEmailService _emailService;

        public AcceptedNotificationRule(IEmailService emailService)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }
        
        public async Task<ServiceResponse> Notify(CakeOrder cakeOrder)
        {
           return  await _emailService.SendEmail(cakeOrder);
        }

        public bool StatusMatch(OrderStatus status)
        {
            return status == OrderStatus.Accepted;
        }
    }
}