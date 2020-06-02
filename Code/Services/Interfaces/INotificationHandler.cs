using System.Threading.Tasks;
using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Models.Helpers;

namespace CloudCityCakeCo.Services.Interfaces
{
    public interface INotificationHandler
    {
        Task<ServiceResponse> SendNotification(CakeOrder cakeOrder);
    }
}