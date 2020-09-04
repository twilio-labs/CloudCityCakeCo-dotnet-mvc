using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCityCakeCo.Models.DTO
{
    public class AuthyLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string AuthyToken { get; set; }
    }
}
