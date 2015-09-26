using System;

namespace PaySharp.InternationalPayments.PayPal.Models
{
    public class PayPalAddress
    {
        public string Street { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
