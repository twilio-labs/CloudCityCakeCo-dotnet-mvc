using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Models.Helpers;
using System.Threading.Tasks;

namespace CloudCityCakeCo.Services.Interfaces
{
    public interface IVerifyService
    {
        Task SendSmsAsync(string phoneNumber);
        Task<ServiceResponse> VerifyPhoneTokenAsync(string phoneNumber, string token);
    }
}