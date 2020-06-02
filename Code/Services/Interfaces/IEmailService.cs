using System.Threading.Tasks;
using CloudCityCakeCo.Models.Entities;
using SendGrid;

namespace CloudCityCakeCo.Services.Interfaces
{
    public interface IEmailService
    {
        Task<Response> SendEmail(CakeOrder cakeOrder);
    }
}