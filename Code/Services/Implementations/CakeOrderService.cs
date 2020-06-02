using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudCityCakeCo.Data.Repositories;
using CloudCityCakeCo.Models.DTO;
using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Models.Enums;
using CloudCityCakeCo.Models.Helpers;
using CloudCityCakeCo.Models.ViewModels;
using CloudCityCakeCo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudCityCakeCo.Services.Implementations
{
    public class CakeOrderService : ICakeOrderService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICakeOrderRepository _cakeOrderRepository;
        private readonly IEmailService _emailService;

        public CakeOrderService(IUserRepository userRepository,
            ICakeOrderRepository cakeOrderRepository,
            IEmailService emailService)
        {
            _userRepository = userRepository?? throw new ArgumentNullException(nameof(userRepository));
            _cakeOrderRepository = cakeOrderRepository?? throw new ArgumentNullException(nameof(cakeOrderRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }
        
        public async Task<ServiceResponse<CakeOrderViewModel>> AddNewOrderAsync(OrderDetails orderDetails)
        {
            var serviceResponse = new ServiceResponse<CakeOrderViewModel>();

            var user = await _userRepository.GetUserByPhoneNumberAsync(orderDetails.Number);

            if (user == null)
            {
                user = new User
                {
                    Name = orderDetails.Name,
                    Number = orderDetails.Number,
                    Email = orderDetails.Email
                };

                user = await _userRepository.AddUserAsync(user);
            }
            
            var cakeOrder = new CakeOrder
            {
                Flavour = orderDetails.Cake.Flavour,
                Frosting = orderDetails.Cake.Frosting,
                Topping = orderDetails.Cake.Topping,
                Size = orderDetails.Cake.Size,
                Price = orderDetails.Cake.Price,
                OrderDate = DateTime.UtcNow,
                UserId =  user.Id,
                OrderStatus = OrderStatus.New
            };
  

            var cakeOrderEntity = await _cakeOrderRepository
                .AddCakeOrderAsync(cakeOrder)
                .ConfigureAwait(false);

            serviceResponse.Content = new CakeOrderViewModel(cakeOrderEntity, user);;
            serviceResponse.ServiceResponseStatus = ServiceResponseStatus.Created;

            return serviceResponse;
        }

        public ServiceResponse<IList<CakeOrderViewModel>> GetAllCakeOrders()
        {
            var serviceResponse = new ServiceResponse<IList<CakeOrderViewModel>>();

            var ordersQuery = _cakeOrderRepository.GetAll();

            var orders = ordersQuery.Include("User").ToList();

            if (orders.Count == 0)
            {
                serviceResponse.ServiceResponseStatus = ServiceResponseStatus.NotFound;
                return serviceResponse;
            }

            var cakeOrders = new List<CakeOrderViewModel>();

            foreach (var order in orders)
            {
                var vm = new CakeOrderViewModel(order, order.User);
                cakeOrders.Add(vm);
            }

            serviceResponse.Content = cakeOrders;
            serviceResponse.Message = "all good";
            serviceResponse.ServiceResponseStatus = ServiceResponseStatus.Ok;

            return serviceResponse;

        }

        public async Task<ServiceResponse<CakeOrderViewModel>> GetOrderByIdAsync(int id)
        {
            var orderEntity = await _cakeOrderRepository
                .GetCakeOrderByIdAsync(id);
            
            var response = new ServiceResponse<CakeOrderViewModel>();

            if (orderEntity is null)
            {
                response.ServiceResponseStatus = ServiceResponseStatus.NotFound;
                response.Message = "no record available";
                return response;
            }
            
            response.Content = new CakeOrderViewModel(orderEntity, orderEntity.User);

            response.ServiceResponseStatus = ServiceResponseStatus.Ok;
            
            return response;
        }

        public async Task<ServiceResponse<CakeOrderViewModel>> UpdateCakeOrderAsync(CakeOrderViewModel cakeOrder)
        {
            var response = new ServiceResponse<CakeOrderViewModel>();

            var entity = await _cakeOrderRepository.GetCakeOrderByIdAsync(cakeOrder.Id);

            entity.OrderStatus = cakeOrder.OrderStatus;

            var cakeOrderEntity = await _cakeOrderRepository.UpdateAsync(entity);

            if (entity.OrderStatus == OrderStatus.Accepted)
            {
               var emailResponse =  await _emailService.SendEmail(entity);
            }

            response.Content = cakeOrder;
            response.ServiceResponseStatus = ServiceResponseStatus.Ok;

            return response;

        }

    }
}