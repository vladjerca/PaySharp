using PaySharp.Exceptions;
using PaySharp.Http;
using PaySharp.Models;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Linq;
using System.Text.RegularExpressions;
using PaySharp.InternationalPayments.Skrill.Models;

namespace PaySharp.InternationalPayments.Skrill.API
{
    internal class SkrillApi
    {
        #region CONFIG
        private bool _isSandboxed;
        private string _releaseEndpoint = "https://www.moneybookers.com/app/payment.pl";
        private string _sandboxEndpoint = "https://www.moneybookers.com/app/test_payment.pl";
        private string _merchantEmail;
        private string _statusUrl;
        #endregion

        public SkrillApi()
        {
            if (!bool.TryParse(ConfigurationManager.AppSettings["SkrillSandboxActive"], out _isSandboxed))
                _isSandboxed = false;

            _statusUrl = ConfigurationManager.AppSettings["SkrillStatusUrl"];
            if (string.IsNullOrWhiteSpace(_statusUrl))
                throw new PaymentConfigurationException("SkrillStatusUrl");

            _merchantEmail = ConfigurationManager.AppSettings["SkrillMerchantEmail"];
            if (string.IsNullOrWhiteSpace(_merchantEmail))
                throw new PaymentConfigurationException("SkrillMerchantEmail");
        }

        public SkrillAuth Checkout(CartModel cart)
        {
            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
            outgoingQueryString["pay_to_email"] = _merchantEmail;
            outgoingQueryString["status_url"] = _statusUrl;
            outgoingQueryString["return_url"] = cart.ReturnURL;
            outgoingQueryString["cancel_url"] = cart.CancelURL;
            outgoingQueryString["pay_from_email"] = cart.ClientEmail;
            outgoingQueryString["language"] = "EN";
            outgoingQueryString["amount"] = cart.Total.ToString();
            outgoingQueryString["currency"] = cart.Currency;
            outgoingQueryString["prepare_only"] = "1";

            string postData = outgoingQueryString.ToString();
            string endpoint = _isSandboxed ? _sandboxEndpoint : _releaseEndpoint;

            var response = Web.Post(endpoint, postData);

            SkrillAuth skrillResponse = new SkrillAuth(response);

            return skrillResponse;
        }
    }
}
