using System.Threading.Tasks;
using CloudCityCakeCo.Models.Entities;

namespace CloudCityCakeCo.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User> GetUserByPhoneNumberAsync(string phoneNumber);
    }
}