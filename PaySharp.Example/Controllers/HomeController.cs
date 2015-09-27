using InternationalPayments.PayPal;
using PaySharp.InternationalPayments.Skrill;
using PaySharp.InternationalPayments.Skrill.Models;
using PaySharp.InternationalPayments.WePay;
using PaySharp.Models;
using System.Web.Mvc;

namespace PaySharp.Example.Controllers
{
    public class HomeController : Controller
    {
        #region Initialize
        public static PayPal payPal = new PayPal();
        public static Skrill skrill = new Skrill();
        public static WePay wePay = new WePay();
        public static CartModel cart;

        public HomeController()
        {
            cart = new CartModel();
            cart.Currency = "USD";
            cart.CancelURL = "http://localhost:29110";
            cart.ClientEmail = "vlad.jerca@yahoo.com";
            cart.Items.Add(new ProductModel
            {
                ProductName = "Test",
                Quantity = 1,
                UnitPrice = 60
            });
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        #region Skrill
        public ActionResult Skrill()
        {
            cart.ReturnURL = "http://localhost:29110/Home/Skrill";
            var result = skrill.Authorize(cart);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Skrill(SkrillCheckout details)
        {
            return Json(details, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PayPal
        public ActionResult PayPal(string token, string payerId)
        {
            if (token == null)
            {
                cart.ReturnURL = "http://localhost:29110/Home/PayPal";
                var result = payPal.Authorize(cart);
                return Redirect(result.CheckoutUrl);
            }
            else
            {
                var result = payPal.Pay("120", token);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region WePay
        public ActionResult WePay(int checkout_id = 0)
        {
            if (checkout_id == 0)
            {
                cart.ReturnURL = "http://localhost:29110/Home/WePay";
                var result = wePay.Authorize(cart);
                return Redirect(result.CheckoutUrl);
            }

            return Json(wePay.Confirm(checkout_id), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}