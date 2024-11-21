using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
    public class CartItem
    {
        public int intCartItemId { get; set; }
        public string strProductId { get; set; }
        public int intSubCategoryId { get; set; }
        public int intCartId { get; set; }
        public int intUserId { get; set; }
        public decimal dcProductPrice { get; set; }
        public int intQuantity { get; set; }
        public string strProductVendorId { get; set; }
        public string strProductColor { get; set; }
        public string strProductSize { get; set; }
        public DateTime dtCreated_on { get; set; }
        public DateTime dtUpdated_on { get;set; }

    }
}
