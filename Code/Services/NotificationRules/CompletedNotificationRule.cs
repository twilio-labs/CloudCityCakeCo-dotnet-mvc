using System;
using System.Threading.Tasks;
using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Models.Enums;
using CloudCityCakeCo.Models.Helpers;
using CloudCityCakeCo.Services.Interfaces;

namespace CloudCityCakeCo.Services.NotificationRules
{
    public class CompletedNotificationRule : IStatusNotificationRule
    {
        private readonly IMessagingService _messagingService;

        public CompletedNotificationRule(IMessagingService messagingService)
        {
            _messagingService = messagingService ?? throw new ArgumentNullException(nameof(messagingService));
        }
        
        public async  Task<ServiceResponse> Notify(CakeOrder cakeOrder)
        {
            return await _messagingService.SendMessage(cakeOrder);
        }

        public bool StatusMatch(OrderStatus status)
        {
            return status == OrderStatus.Completed;
        }
    }
}