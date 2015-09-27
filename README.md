# PaySharp
Payments made easy.
![Darkroom](https://media.licdn.com/mpr/mpr/jc/AAEAAQAAAAAAAAXhAAAAJGI5ZjVhN2ZmLThmNWQtNGVkYy04NTI2LTAzMjgzNjgwZTg4Nw.jpg)

### Configuration

```xml
  <appSettings>
    <add key="webpages:Version" value="3.0.0.0"/>
    <add key="webpages:Enabled" value="false"/>
    <add key="ClientValidationEnabled" value="true"/>
    <add key="UnobtrusiveJavaScriptEnabled" value="true"/>
    
    <!--PAYPAL CREDENTIALS-->
    <add key="PayPalUsername" value="XXXXXX"/>
    <add key="PayPalPassword" value="XXXXXX"/>
    <add key="PayPalSignature" value="XXXXXX"/>
    <add key="PayPalSandboxActive" value="true" />
    
    <!--SKRILL CREDENTIALS-->
    <add key="SkrillSandboxActive" value="true"/>
    <add key="SkrillStatusUrl" value="http://localhost:29110/Home/Skrill"/>
    <add key="SkrillMerchantEmail" value="demoqco@sun‐fish.com"/>

    <!--WEPAY CREDENTIALS-->
    <add key="WePayClientID" value="XXXXXX"/>
    <add key="WePayClientSecret" value="XXXXXX"/>
    <add key="WePayAccessToken" value="XXXXXX"/>
    <add key="WePayAccountID" value="XXXXXX"/>
    <add key="WePaySandboxActive" value="true" />
  </appSettings>
```

### PayPal integration

```csharp
        public ActionResult PayPal(string token, string payerId)
        {
            // on first run get authorization from paypal
            if (token == null)
            {
                cart.ReturnURL = "http://localhost:29110/Home/PayPal";
                var result = payPal.Authorize(cart);
                return Redirect(result.CheckoutUrl);
            }
            // after authorization finalize and get all the payment details
            else
            {
                var result = payPal.Pay("120", token);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
```

### Skrill integration

```csharp
        // start checkout
        public ActionResult Skrill()
        {
            cart.ReturnURL = "http://localhost:29110/Home/Skrill";
            var result = skrill.Authorize(cart);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // after skrill returns payment it will contact this endpoint (as set up in the configuration)
        [HttpPost]
        public ActionResult Skrill(SkrillCheckout details)
        {
            return Json(details, JsonRequestBehavior.AllowGet);
        }
```

### WePay integration

```csharp
        public ActionResult WePay(int checkout_id = 0)
        {
            // authorize and redirect user to payment page
            if (checkout_id == 0)
            {
                cart.ReturnURL = "http://localhost:29110/Home/WePay";
                var result = wePay.Authorize(cart);
                return Redirect(result.CheckoutUrl);
            }
            // after payment check the payment status
            return Json(wePay.Confirm(checkout_id), JsonRequestBehavior.AllowGet);
        }
        
        // NOTE: More to come for WePay (payment details, etc...)
```
