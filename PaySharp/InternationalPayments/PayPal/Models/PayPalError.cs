namespace PaySharp.InternationalPayments.PayPal.Models
{
    public class PayPalError
    {
        public string ErrorCode { get; set; }
        public string ShortMessage { get; set; }
        public string Message { get; set; }
    }
}
