using PaySharp.Http.Models;
using PaySharp.InternationalPayments.Skrill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PaySharp.InternationalPayments.WePay.Models
{
    public class WePayAuth
    {
        public string CheckoutUrl { get; set; }
        public string Token { get; set; }
        public bool Success { get; set; }
        public WePayError Error { get; set; }

        internal WePayAuth(HttpResponse response)
        {
            var jsonResponse = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(response.Response);

            var error = jsonResponse.Keys.Where(x => x.Contains("error")).FirstOrDefault();

            if (error == null)
            {
                CheckoutUrl = jsonResponse["checkout_uri"].ToString();
                Token = jsonResponse["checkout_id"].ToString();
                Success = true;
            }
            else
            {
                Error = new WePayError();
                Error.ShortMessage = jsonResponse["error"].ToString();
                Error.Message = jsonResponse["error_description"].ToString();
                Error.Code = Convert.ToInt32(jsonResponse["error_code"]);
                Success = false;
            }
        }
    }
}
