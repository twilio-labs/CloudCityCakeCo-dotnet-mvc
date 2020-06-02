using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Models.Helpers;
using CloudCityCakeCo.Services.Interfaces;
using CloudCityCakeCo.Services.NotificationRules;

namespace CloudCityCakeCo.Services.Implementations
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly IList<IStatusNotificationRule> _notificationRules;

        public NotificationHandler(IEnumerable<IStatusNotificationRule> notificationRules)
        {
            _notificationRules = notificationRules.ToList();
        }
        
        public async Task<ServiceResponse> SendNotification(CakeOrder cakeOrder)
        {
            var serviceResponse = new ServiceResponse();

            IStatusNotificationRule rule = _notificationRules
                .FirstOrDefault(r => r.StatusMatch(cakeOrder.OrderStatus));

            if (rule != null)
            {
                serviceResponse = await rule.Notify(cakeOrder);
            }

            return serviceResponse;
        }
    }
}