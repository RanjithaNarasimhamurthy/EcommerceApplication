using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
    public class ProductResponse
    {
        public string strProductId { get; set; }
        public int intSubCategoryId { get; set; }
        public string strProductVendorId { get; set; }
        public string strProductName { get; set; }
        public string strProductDescription { get; set; }
        public string strBrand { get; set; }
        public decimal dcMRP { get; set; }
        public bool bitIsVisible { get; set; }
        public bool bitIsDeleted { get; set; }
        public bool bitIsWishlist { get; set; }
        public DateTime dtCreated_on { get; set; }
        public DateTime dtUpdated_on { get; set; }
        public List<ProductImages> ProductImages { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public double AverageRating { get; set; }
    }
}
