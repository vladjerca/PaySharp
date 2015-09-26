using PaySharp.Exceptions;
using PaySharp.InternationalPayments.Skrill.API;
using PaySharp.InternationalPayments.Skrill.Models;
using PaySharp.Models;

namespace PaySharp.InternationalPayments.Skrill
{
    /*
        Skrill developer page: https://www.skrill.com/en/skrill-integration/
    */
    public class Skrill
    {
        SkrillApi skrillApi;

        public Skrill()
        {
            skrillApi = new SkrillApi();
        }

        public SkrillAuth Authorize(CartModel model)
        {
            var response = skrillApi.Checkout(model);

            if (!response.Success)
                throw new PaymentException(response.Error.Message);

            return response;
        }
    }
}
