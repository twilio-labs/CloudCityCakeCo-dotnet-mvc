using System.Threading.Tasks;
using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Models.Helpers;

namespace CloudCityCakeCo.Services.Interfaces
{
    public interface IMessagingService
    {
        Task<ServiceResponse> SendMessage(CakeOrder cakeOrder);
      
    }
}