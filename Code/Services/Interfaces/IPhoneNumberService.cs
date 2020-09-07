using CloudCityCakeCo.Models.DTO;
using CloudCityCakeCo.Models.Helpers;
using System.Threading.Tasks;

namespace CloudCityCakeCo.Services.Interfaces
{
    public interface IPhoneNumberService
    {
        Task<ServiceResponse<PhoneNumberTransferResource>> LookupNumber(string phoneNumber);
    }
}
