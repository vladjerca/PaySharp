using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySharp.Models
{
    public class CartModel
    {
        public string VendorName { get; set; }
        public string ClientEmail { get; set; }
        public string Currency { get; set; }
        public string ReturnURL { get; set; }
        public string CancelURL { get; set; }

        public List<ProductModel> Items { get; set; }

        public CartModel()
        {
            Items = new List<ProductModel>();
        }

        public double Total
        {
            get
            {
                double amount = 0;
                Items.ForEach((x) => {
                    amount += x.Quantity * x.UnitPrice;
                });
                return amount;
            }
        }

        public string Description
        {
            get
            {
                StringBuilder description = new StringBuilder("Product prices:");
                Items.ForEach((x) => {
                    description.AppendLine(string.Format("{0} - {1} x {2}{3}", x.ProductName, x.UnitPrice, x.Quantity, Currency));
                });

                return description.ToString();
            }
        }
    }
}
