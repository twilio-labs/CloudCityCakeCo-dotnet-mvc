using System;
using System.Threading.Tasks;
using CloudCityCakeCo.Data;
using CloudCityCakeCo.Models.DTO;
using CloudCityCakeCo.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudCityCakeCo.ApiControllers
{
    [ApiController]
    [Route("api/order")]
    public class CakeOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CakeOrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET
        [HttpGet]
        public IActionResult Get()
        {
            return null;
        }
        
        
        // POST
        [HttpPost]
        public async Task<IActionResult> Post(OrderDetails orderDetails)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(e => e.Number == orderDetails.Number);

            if (user == null)
            {
                user = new User
                {
                    Name = orderDetails.Name,
                    Number = orderDetails.Number,
                    Email = orderDetails.Email
                };

                var entity = await _context.Users.AddAsync(user);

                await _context.SaveChangesAsync();
                user = entity.Entity;
            }
            
            var cakeOrder = new CakeOrder
            {
                Flavour = orderDetails.Cake.Flavour,
                Frosting = orderDetails.Cake.Frosting,
                Topping = orderDetails.Cake.Topping,
                Size = orderDetails.Cake.Size,
                Price = orderDetails.Cake.Price,
                OrderDate = DateTime.UtcNow,
                UserId =  user.Id
            };

            var cakeEntity = await _context
                .CakeOrders.AddAsync(cakeOrder);

            await _context.SaveChangesAsync();
            
            
            return new JsonResult(cakeEntity.Entity.Id.ToString());
        }
    }
}