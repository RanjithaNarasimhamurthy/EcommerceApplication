using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
    public class Product
    {
        public string strProductId { get; set; }
        public int intSubCategoryId { get; set; }
        public string strProductVendorId { get; set; }
        public string strProductName { get; set; }
        public string strProductDescription { get; set; }
        public string strBrand { get; set; }
        public decimal dcMRP { get; set; }
        //public int intRating { get; set; }
        public bool IsVisible { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsWishlist { get; set; }
        public DateTime? dtCreated_On { get; set; }
        public DateTime? dtUpdated_On { get; set; }

        public List<ProductImages> ProductImageDetails { get; set; }

        [NotMapped]
        public List<ProductImages>? ProductImagesobj { get; set; }
        [NotMapped]
        public ProductImages? ProductImageDetailsobj { get; set; }

        //[NotMapped]
        //public List<ProductImages>? ProductImageDetails { get; set; } = new List<ProductImages>();

        [NotMapped]
        public byte[]? vbImage { get; set; }

        
    }
}
