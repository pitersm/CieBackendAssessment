using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CieBackendAssessment
{
    public class StripeConfig
    {
        public string PublicApiKey { get; set; }
        public string SecretKey { get; set; }
        public string PriceId { get; set; }
    }
}
