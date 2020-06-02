using System;
using CloudCityCakeCo.Models.Enums;

namespace CloudCityCakeCo.Models.Entities
{
    public class CakeOrder
    {
        public int Id { get; set; }
        public string Topping { get; set; }
        public string Frosting { get; set; }
        public string Flavour { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}