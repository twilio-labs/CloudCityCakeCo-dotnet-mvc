using CloudCityCakeCo.Models.Entities;

namespace CloudCityCakeCo.Models.ViewModels
{
    public class CakeViewModel
    {
        public CakeViewModel(){ }
        
        public CakeViewModel(CakeOrder cake)
        {
            Topping = cake.Topping;
            Frosting = cake.Frosting;
            Flavour = cake.Flavour;
            Size = cake.Size;
            Price = cake.Price;
        }
        
        public string Topping { get; set; }

        public string Frosting { get; set; }

        public string Flavour { get; set; }

        public string Size { get; set; }

        public decimal Price { get; set; }
    }
}