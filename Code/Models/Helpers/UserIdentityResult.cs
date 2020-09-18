using CloudCityCakeCo.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCityCakeCo.Models.Helpers
{
    public class UserIdentityResult
    {
        public IdentityResult IdentityResult { get; set; }
        public User User { get; set; }
    }
}
