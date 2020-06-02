namespace CloudCityCakeCo.Models.DTO
{
    public class OrderDetails
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Cake Cake { get; set; }
    }
}