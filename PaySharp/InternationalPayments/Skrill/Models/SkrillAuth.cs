using PaySharp.Http.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace PaySharp.InternationalPayments.Skrill.Models
{
    public class SkrillAuth
    {
        public string CheckoutUrl { get; set; }
        public string Token { get; set; }
        public bool Success { get; set; }
        public SkrillError Error { get; set; }

        internal SkrillAuth(HttpResponse response)
        {
            Success = true;

            if (response.StatusCode != 200)
                SetError("Error connecting to Skrill payment endpoint.", string.Format("Additional information: {0}", response.ToString()));
            else
            {
                string sessionHeader = response.Headers
                                                .Where(x => x.Key.Contains("Set-Cookie") && x.Value.Contains("SESSION_ID"))
                                                .Select(x => x.Value)
                                                .FirstOrDefault();

                string statusHeader = response.Headers
                                                .Where(x => x.Key == "X-skrill-status")
                                                .Select(x => x.Value)
                                                .FirstOrDefault();

                if(statusHeader != null && statusHeader.Contains("error"))
                    SetError(statusHeader);
                
                if (sessionHeader == null)
                    SetError("SESSION_ID error.", "Set-Cookie header could not be found in response.");

                var match = Regex.Match(sessionHeader, @"(?<=SESSION_ID=).*(?=;\spath=)");

                if (!match.Success)
                    SetError("SESSION_ID not found.", "Request did not yield the SESSION_ID.");
                else
                {
                    Token = match.Value;
                    CheckoutUrl = string.Format("{0}?sid={1}", response.Host, Token);
                }
            }
        }

        private void SetError(string shortMessage, string message = "")
        {
            Success = false;
            Error = new SkrillError();
            Error.ShortMessage = shortMessage;
            Error.Message = message;
        }
    }
}
