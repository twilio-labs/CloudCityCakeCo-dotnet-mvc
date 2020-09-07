using System;
using System.Collections;
using System.Threading.Tasks;
using CloudCityCakeCo.Models.ViewModels;
using CloudCityCakeCo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudCityCakeCo.Controllers
{
    [Authorize]
    [Route("/orders")]
    public class AdminController : Controller
    {
        private readonly ICakeOrderService _cakeOrderService;

        public AdminController(ICakeOrderService cakeOrderService)
        {
            _cakeOrderService = cakeOrderService ?? throw new ArgumentNullException(nameof(cakeOrderService));
        }

        // GET
        [Route("/orders")]
        public IActionResult Index()
        {
            var cakeOrders = _cakeOrderService.GetAllCakeOrders();
            return View(cakeOrders.Content);
        }

        [HttpGet]
        [Route("/orders/update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var cakeOrder = await _cakeOrderService.GetOrderByIdAsync(id);
            
            return View(cakeOrder.Content);
        }
        
        [HttpPost]
        [Route("/orders/update")]
        public async Task<IActionResult> Update(CakeOrderViewModel cakeOrder)
        {
            var _ = await _cakeOrderService.UpdateCakeOrderAsync(cakeOrder);
            
            return RedirectToAction("Index");
        }

    }
}