using System;
using System.Collections.Specialized;

namespace PaySharp.InternationalPayments.PayPal.Models
{
    public class PayPalDetails : PayPalResponse
    {
        public string PayerId { get; set; }
        public PayPalAddress Address { get; set; }

        public PayPalDetails(NameValueCollection decodedResponse)
            : base(decodedResponse)
        {
            string strAck = decodedResponse["ACK"].ToLower();

            if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
            {
                PayerId = decodedResponse["PAYERID"];

                Address = new PayPalAddress();
                Address.Street = decodedResponse["SHIPTOSTREET"].ToString();
                Address.Zip = decodedResponse["SHIPTOZIP"].ToString();
                Address.Email = decodedResponse["EMAIL"].ToString();
                Address.Country = decodedResponse["SHIPTOCOUNTRYCODE"].ToString();
                Address.City = decodedResponse["SHIPTOCITY"].ToString();
                Address.Timestamp = Convert.ToDateTime(decodedResponse["TIMESTAMP"].ToString());
            }
        }
    }
}
