using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Models.Helpers;
using System.Threading.Tasks;

namespace CloudCityCakeCo.Services.Interfaces
{
    public interface IAuthyService
    {
        Task<string> RegisterUserAsync(User user);
        Task<string> SendSmsAsync(string authyId);
        Task<TokenVerificationResult> VerifyPhoneTokenAsync(string authyId, string token);
    }
}