using System.Linq;
using System.Threading.Tasks;
using CloudCityCakeCo.Models.Entities;

namespace CloudCityCakeCo.Data.Repositories
{
    public interface ICakeOrderRepository
    {
        IQueryable<CakeOrder> GetAll();
        Task<CakeOrder> AddCakeOrderAsync(CakeOrder cakeOrder);
        Task<CakeOrder> GetCakeOrderByIdAsync(int id);
        Task<CakeOrder> UpdateAsync(CakeOrder cakeOrder);
    }
}