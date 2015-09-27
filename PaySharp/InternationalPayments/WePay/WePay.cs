using PaySharp.Exceptions;
using PaySharp.InternationalPayments.WePay.API;
using PaySharp.InternationalPayments.WePay.Models;
using PaySharp.Models;

namespace PaySharp.InternationalPayments.WePay
{
    public class WePay
    {
        /*
        WePay developer page: https://wepay.com/developer
        */
        WePayApi wePay;

        public WePay()
        {
            wePay = new WePayApi();
        }

        public WePayAuth Authorize(CartModel model)
        {
            var response = wePay.Checkout(model);

            if (!response.Success)
                throw new PaymentException(response.Error.Message);

            return response;
        }

        public dynamic Confirm(int checkoutId)
        {
            var response = wePay.CheckoutDetails(checkoutId);

            return response;
        }
    }
}
