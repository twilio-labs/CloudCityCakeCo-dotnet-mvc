using System;
using System.Linq;
using System.Threading.Tasks;
using CloudCityCakeCo.Models.Enums;
using CloudCityCakeCo.Models.Helpers;
using CloudCityCakeCo.Models.ViewModels;
using CloudCityCakeCo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CloudCityCakeCo.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICakeOrderService _cakeOrderService;

        public AdminController(ICakeOrderService cakeOrderService)
        {
            _cakeOrderService = cakeOrderService ?? throw new ArgumentNullException(nameof(cakeOrderService));
        }
        
        // GET
        public IActionResult Index()
        {
            var cakeOrders = _cakeOrderService.GetAllCakeOrders();
            return View(cakeOrders.Content);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var cakeOrder = await _cakeOrderService.GetOrderByIdAsync(id);
            
            return View(cakeOrder.Content);
        }
        
        [HttpPost]
        public async Task<IActionResult> Update(CakeOrderViewModel cakeOrder)
        {
            var _ = await _cakeOrderService.UpdateCakeOrderAsync(cakeOrder);
            
            return RedirectToAction("Index");
        }

    }
}