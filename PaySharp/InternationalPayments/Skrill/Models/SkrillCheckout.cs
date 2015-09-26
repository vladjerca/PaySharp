using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySharp.InternationalPayments.Skrill.Models
{
    public class SkrillCheckout
    {
        /// <summary>
        /// Your email address.
        /// </summary>
        public string pay_to_email { get; set; }
        /// <summary>
        /// Email address of the customer who is  making the payment.
        /// </summary>
        public string pay_from_email { get; set; }
        /// <summary>
        /// Unique ID of your Skrill account. ONLY  needed for the calculation of the MD5 signature.
        /// </summary>
        public int merchant_id { get; set; }
        /// <summary>
        /// Unique ID of the customer’s Skrill  account.
        /// </summary>
        public int customer_id { get; set; }
        /// <summary>
        /// A unique reference or identification  number provided by you in your HTML form. 
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// The total amount of the payment in the currency of your Skrill Digital Wallet account.
        /// </summary>
        public int mb_amount { get; set; }
        /// <summary>
        /// Currency of mb_amount. Will always  be the same as the currency of your  Skrill Digital Wallet account.
        /// </summary>
        public string mb_currency { get; set; }
        /// <summary>
        /// Status of the transaction: ‐2 failed / 2  processed / 0 pending / ‐1 cancelled
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// If the transaction is with status ‐2  (failed), this field will contain a code  detailing the reason for the failure.
        /// </summary>
        public string failed_reason_code { get; set; }
        /// <summary>
        /// MD5 signature.
        /// </summary>
        public string md5sig { get; set; }
        /// <summary>
        /// SHA2 signature.
        /// </summary>
        public string sha2sig { get; set; }
        /// <summary>
        /// Amount of the payment as posted in  your HTML form.
        /// </summary>
        public double amount { get; set; }
        /// <summary>
        /// Currency of the payment as posted in  your HTML form.
        /// </summary>
        public string currency { get; set; }
        /// <summary>
        /// The payment method the customer used. 
        /// </summary>
        public string payment_type { get; set; }
    }
}
