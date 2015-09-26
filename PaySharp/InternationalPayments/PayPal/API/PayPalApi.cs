using PaySharp.Exceptions;
using PaySharp.Http;
using PaySharp.InternationalPayments.PayPal.Models;
using PaySharp.Models;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;

namespace PaySharp.InternationalPayments.PayPal.API
{
    internal class PayPalApi
    {
        #region CONFIG
        // sandbox flags
        private static bool _isSandboxed;

        // live urls
        private static string _releaseEndpoint = "https://api-3t.paypal.com/nvp";
        private static string _releaseHost = "www.paypal.com";

        // sandbox urls
        private static string _sandboxEndpoint = "https://api-3t.sandbox.paypal.com/nvp";
        private static string _sandboxHost = "www.sandbox.paypal.com";

        // determined urls
        private static string _endpoint;
        private static string _host;

        // merchant details
        private string _ppUsername { get; set; }
        private string _ppPassword { get; set; }
        private string _ppSignature { get; set; }
        public string _ppCurrency { get; set; }
        #endregion

        public PayPalApi()
        {

            if (!bool.TryParse(ConfigurationManager.AppSettings["PayPalSandboxActive"], out _isSandboxed))
                throw new PaymentConfigurationException("PayPalSandboxActive");

            _ppUsername = ConfigurationManager.AppSettings["PayPalUsername"];
            if (string.IsNullOrWhiteSpace(_ppUsername))
                throw new PaymentConfigurationException("PayPalUsername");

            _ppPassword = ConfigurationManager.AppSettings["PayPalPassword"];
            if (string.IsNullOrWhiteSpace(_ppPassword))
                throw new PaymentConfigurationException("PayPalPassword");

            _ppSignature = ConfigurationManager.AppSettings["PayPalSignature"];
            if (string.IsNullOrWhiteSpace(_ppSignature))
                throw new PaymentConfigurationException("PayPalSignature");

            _endpoint = _isSandboxed ? _sandboxEndpoint : _releaseEndpoint;
            _host = _isSandboxed ? _sandboxHost : _releaseHost;
        }

        public PayPalAuth Checkout(CartModel cart)
        {
            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);

            outgoingQueryString["METHOD"] = "SetExpressCheckout";
            outgoingQueryString["RETURNURL"] = cart.ReturnURL;
            outgoingQueryString["CANCELURL"] = cart.CancelURL;
            outgoingQueryString["BRANDNAME"] = cart.VendorName;
            outgoingQueryString["PAYMENTREQUEST_0_AMT"] = cart.Total.ToString();
            outgoingQueryString["PAYMENTREQUEST_0_ITEMAMT"] = cart.Total.ToString();
            outgoingQueryString["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
            outgoingQueryString["PAYMENTREQUEST_0_CURRENCYCODE"] = _ppCurrency;

            for (int i = 0; i < cart.Items.Count; i++)
            {
                outgoingQueryString["L_PAYMENTREQUEST_0_NAME" + i] = cart.Items[i].ProductName.ToString();
                outgoingQueryString["L_PAYMENTREQUEST_0_AMT" + i] = cart.Items[i].UnitPrice.ToString();
                outgoingQueryString["L_PAYMENTREQUEST_0_QTY" + i] = cart.Items[i].Quantity.ToString();
            }

            outgoingQueryString.Add(AuthQuery);

            var result = Web.Post(_endpoint, outgoingQueryString.ToString(), ContentType.Default);

            var decodedResponse = HttpUtility.ParseQueryString(result.Response);
            decodedResponse["HOST"] = _host;

            return new PayPalAuth(decodedResponse);
        }

        public PayPalDetails CheckoutDetails(string token)
        {
            if (_isSandboxed)
            {
                _releaseEndpoint = _sandboxEndpoint;
            }

            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(string.Empty);

            outgoingQueryString["METHOD"] = "GetExpressCheckoutDetails";
            outgoingQueryString["TOKEN"] = token;
            outgoingQueryString.Add(AuthQuery);

            var result = Web.Post(_endpoint, outgoingQueryString.ToString());

            var decodedResponse = HttpUtility.ParseQueryString(result.Response);

            return new PayPalDetails(decodedResponse);
        }

        public PayPalCheckout Finalize(string finalPaymentAmount, string token, string payerId)
        {
            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(string.Empty);
            outgoingQueryString["METHOD"] = "DoExpressCheckoutPayment";
            outgoingQueryString["TOKEN"] = token;
            outgoingQueryString["PAYERID"] = payerId;
            outgoingQueryString["PAYMENTREQUEST_0_AMT"] = finalPaymentAmount;
            outgoingQueryString["PAYMENTREQUEST_0_CURRENCYCODE"] = _ppCurrency;
            outgoingQueryString["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";

            outgoingQueryString.Add(AuthQuery);

            var result = Web.Post(_endpoint, outgoingQueryString.ToString());

            NameValueCollection decodedResponse = HttpUtility.ParseQueryString(result.Response);

            return new PayPalCheckout(decodedResponse);
        }

        private NameValueCollection AuthQuery
        {
            get
            {
                NameValueCollection authQueryString = HttpUtility.ParseQueryString(String.Empty);
                authQueryString["USER"] = _ppUsername;
                authQueryString["PWD"] = _ppPassword;
                authQueryString["SIGNATURE"] = _ppSignature;
                authQueryString["SUBJECT"] = "";
                authQueryString["VERSION"] = "88.0";
                authQueryString["BUTTONSOURCE"] = "PP-ECWizard";
                return authQueryString;
            }
        }
    }
 }
