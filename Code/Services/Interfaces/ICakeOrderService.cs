using System.Collections.Generic;
using System.Threading.Tasks;
using CloudCityCakeCo.Models.DTO;
using CloudCityCakeCo.Models.Helpers;
using CloudCityCakeCo.Models.ViewModels;

namespace CloudCityCakeCo.Services.Interfaces
{
    public interface ICakeOrderService
    {
        Task<ServiceResponse<CakeOrderViewModel>> AddNewOrderAsync(OrderDetails orderDetails);
        ServiceResponse<IList<CakeOrderViewModel>> GetAllCakeOrders();

        Task<ServiceResponse<CakeOrderViewModel>> GetOrderByIdAsync(int id);
        Task<ServiceResponse<CakeOrderViewModel>> UpdateCakeOrderAsync(CakeOrderViewModel cakeOrder);
    }
}