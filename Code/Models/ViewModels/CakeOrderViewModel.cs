using System;
using System.Collections.Generic;
using System.Globalization;
using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CloudCityCakeCo.Models.ViewModels
{
    public class CakeOrderViewModel
    {
        public CakeOrderViewModel(){}
        
        public CakeOrderViewModel (CakeOrder cakeOrder)
        {
            Id = cakeOrder.Id;
            OrderDate = cakeOrder.OrderDate.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
            CompletedDate = ConvertDateTime(cakeOrder.CompletedDate);
            OrderStatus = cakeOrder.OrderStatus;
            UserId = cakeOrder.UserId;

        }
        public CakeOrderViewModel (CakeOrder cakeOrder, User user)
        {
            Id = cakeOrder.Id;
            OrderDate = cakeOrder.OrderDate.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
            CompletedDate = ConvertDateTime(cakeOrder.CompletedDate);
            OrderStatus = cakeOrder.OrderStatus;
            UserId = cakeOrder.UserId;
            UserName = user.Name;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
            Cake = new CakeViewModel(cakeOrder);

        }
        
        public int Id { get; set; }
        
        public string OrderDate { get; set; }
        public string CompletedDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int UserId { get; set; }
        
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        
        public CakeViewModel Cake { get; set; }
       

        private string ConvertDateTime(DateTime? date)
        {
            if (date == null)
            {
                return "";
            }

            var validDate = (DateTime) date;

            return validDate.ToShortDateString();
        }
    }
}