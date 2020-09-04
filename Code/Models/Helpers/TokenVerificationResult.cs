using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCityCakeCo.Models.Helpers
{
    public class TokenVerificationResult
    {
        public TokenVerificationResult(string message, bool succeeded = true)
        {
            this.Message = message;
            this.Succeeded = succeeded;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
