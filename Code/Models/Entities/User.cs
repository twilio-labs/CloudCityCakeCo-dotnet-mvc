using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CloudCityCakeCo.Models.Entities
{
    public class User : IdentityUser<int>
    {

        [PersonalData]
        public string Number { get; set; }
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public string CountryCode { get; set; }
        [PersonalData]
        public string AuthyId { get; set; }
        [PersonalData]
        public virtual IList<CakeOrder> CakeOrders { get; set; }
    }



    public class ApplicationRole : IdentityRole<int>
    {
    }
}

