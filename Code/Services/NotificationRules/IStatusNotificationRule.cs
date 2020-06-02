using System.Threading.Tasks;
using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Models.Enums;
using CloudCityCakeCo.Models.Helpers;

namespace CloudCityCakeCo.Services.NotificationRules
{
    public interface IStatusNotificationRule
    {
        Task<ServiceResponse> Notify(CakeOrder cakeOrder);

        bool StatusMatch(OrderStatus status);
    }
}