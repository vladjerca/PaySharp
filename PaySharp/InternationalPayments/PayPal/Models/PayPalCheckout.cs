using System.Collections.Specialized;

namespace PaySharp.InternationalPayments.PayPal.Models
{
    public class PayPalCheckout : PayPalResponse
    {
        public PayPalAddress Address { get; set; }
        public string TransactionId { get; set; }

        internal PayPalCheckout(NameValueCollection decodedResponse) 
            : base(decodedResponse)
        {
            string strAck = decodedResponse["ACK"].ToLower();
            if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
            {
                TransactionId = decodedResponse["PAYMENTINFO_0_TRANSACTIONID"].ToString();
            }
        }
    }
}
