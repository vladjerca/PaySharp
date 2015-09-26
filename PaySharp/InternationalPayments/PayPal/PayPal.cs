using PaySharp.Exceptions;
using PaySharp.InternationalPayments.PayPal.API;
using PaySharp.InternationalPayments.PayPal.Models;
using PaySharp.Models;

namespace InternationalPayments.PayPal
{
    public class PayPal
    {
        PayPalApi payPalApi;

        public PayPal()
        {
            payPalApi = new PayPalApi();
        }

        public PayPalAuth Authorize(CartModel model)
        {
            payPalApi._ppCurrency = model.Currency;

            var response = payPalApi.Checkout(model);

            if (!response.Success)
                throw new PaymentException(response.Error.Message);

            return response;
        }

        public PayPalCheckout Pay(string total, string token)
        {
            var detailsResponse = payPalApi.CheckoutDetails(token);

            if (!detailsResponse.Success)
                throw new PaymentException(detailsResponse.Error.Message);

            var response = payPalApi.Finalize(total, token, detailsResponse.PayerId);

            if (!response.Success)
                throw new PaymentException(detailsResponse.Error.Message);

            response.Address = detailsResponse.Address;
            return response;
        }
    }
}
