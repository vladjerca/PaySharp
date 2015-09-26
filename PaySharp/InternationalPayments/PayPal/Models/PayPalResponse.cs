using System.Collections.Specialized;

namespace PaySharp.InternationalPayments.PayPal.Models
{
    public class PayPalResponse
    {
        public bool Success { get; set; }
        public PayPalError Error { get; set; }

        internal PayPalResponse(NameValueCollection decodedResponse) 
        {
            string strAck = decodedResponse["ACK"].ToLower();
            if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
            {
                Success = true;
            }
            else
            {
                Error = new PayPalError();
                Error.ErrorCode = decodedResponse["L_ERRORCODE0"];
                Error.ShortMessage = decodedResponse["L_SHORTMESSAGE0"];
                Error.Message = decodedResponse["L_LONGMESSAGE0"];
                Success = false;
            }
        }
    }
}
