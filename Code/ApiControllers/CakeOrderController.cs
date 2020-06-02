using System;
using System.Threading.Tasks;
using CloudCityCakeCo.Models.DTO;
using CloudCityCakeCo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudCityCakeCo.ApiControllers
{
    [ApiController]
    [Route("api/order")]
    public class CakeOrderController : ControllerBase
    {
        private readonly ICakeOrderService _cakeOrderService;
        public CakeOrderController(ICakeOrderService cakeOrderService)
        {
            _cakeOrderService = cakeOrderService ?? throw new ArgumentNullException(nameof(cakeOrderService));
        }
 
        // POST
        [HttpPost]
        public async Task<IActionResult> Post(OrderDetails orderDetails)
        {
            var cakeOrderResponse = await _cakeOrderService
                .AddNewOrderAsync(orderDetails);
            return new JsonResult(cakeOrderResponse.Content.Id.ToString());
        }
    }
}