using System.Collections.Specialized;

namespace PaySharp.InternationalPayments.PayPal.Models
{
    public class PayPalAuth : PayPalResponse
    {
        public string CheckoutUrl { get; set; }
        public string Token { get; set; }

        public PayPalAuth(NameValueCollection decodedResponse)
            : base(decodedResponse)
        {
            string strAck = decodedResponse["ACK"].ToLower();
            if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
            {
                Token = decodedResponse["TOKEN"];
                CheckoutUrl = "https://" + decodedResponse["HOST"] + "/cgi-bin/webscr?cmd=_express-checkout" + "&token=" + Token;
            }
        }
    }
}
