using System.Collections.Generic;

namespace CloudCityCakeCo.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public virtual IList<CakeOrder> CakeOrders { get; set; }
    }
}