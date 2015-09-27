using PaySharp.Exceptions;
using PaySharp.Http;
using PaySharp.InternationalPayments.WePay.Models;
using PaySharp.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace PaySharp.InternationalPayments.WePay.API
{
    internal class WePayApi
    {
        #region CONFIG
        private bool _isSandboxed;
        private string _releaseEndpoint = "https://wepayapi.com/v2/";
        private string _sandboxEndpoint = "https://stage.wepayapi.com/v2/";
        private string _endpoint;
        private string _clientId;
        private string _accountId;
        private string _clientSecret;
        private string _accessToken;
        #endregion

        internal WePayApi()
        {
            if (!bool.TryParse(ConfigurationManager.AppSettings["WePaySandboxActive"], out _isSandboxed))
                _isSandboxed = false;

            _accountId = ConfigurationManager.AppSettings["WePayAccountID"];
            if (string.IsNullOrWhiteSpace(_accountId))
                throw new PaymentConfigurationException("WePayAccountID");

            _clientId = ConfigurationManager.AppSettings["WePayClientID"];
            if (string.IsNullOrWhiteSpace(_clientId))
                throw new PaymentConfigurationException("WePayClientID");

            _clientSecret = ConfigurationManager.AppSettings["WePayClientSecret"];
            if (string.IsNullOrWhiteSpace(_clientSecret))
                throw new PaymentConfigurationException("WePayClientSecret");

            _accessToken = ConfigurationManager.AppSettings["WePayAccessToken"];
            if (string.IsNullOrWhiteSpace(_accessToken))
                throw new PaymentConfigurationException("WePayAccessToken");

            _endpoint = _isSandboxed ? _sandboxEndpoint : _releaseEndpoint;
        }

        internal WePayAuth Checkout(CartModel cart)
        {
            NameValueCollection requestData = HttpUtility.ParseQueryString(string.Empty);
            requestData["account_id"] = _accountId;
            requestData["amount"] = cart.Total.ToString();
            requestData["type"] = "service";
            requestData["currency"] = cart.Currency;
            requestData["short_description"] = cart.Description;
            requestData["redirect_uri"] = cart.ReturnURL;

            var response = Web.Post(
                                    string.Format("{0}{1}", _endpoint, "checkout/create"), 
                                    requestData, 
                                    ContentType.Json, 
                                    WePayHeaders());

            return new WePayAuth(response);
        }

        internal bool CheckoutDetails(int checkoutId)
        {
            NameValueCollection requestData = HttpUtility.ParseQueryString(string.Empty);
            requestData["checkout_id"] = checkoutId.ToString();

            var response = Web.Post(
                                    string.Format("{0}{1}", _endpoint, "checkout"),
                                    requestData,
                                    ContentType.Json,
                                    WePayHeaders());
            bool result = false;
            var jsonResponse = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(response.Response);
            dynamic state = jsonResponse.Where(x => x.Key == "state").Select(x => x.Value).FirstOrDefault();

            if (state != null)
            {
                state = Convert.ToString(state);
                if (state != "failed" && state != "expired")
                    result = true;
            }

            return result;
        }

        private NameValueCollection WePayHeaders()
        {
            NameValueCollection headers = new NameValueCollection();
            headers["Authorization"] = string.Format("Bearer {0}", _accessToken);
            headers["Api-Version"] = "2014-01-08";
            return headers;
        }
    }
}
