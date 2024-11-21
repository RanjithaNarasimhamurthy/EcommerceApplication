using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
    public class CartResponseModel
    {
        // Properties from TBL_Product_Details
        public int IntCartItemId { get; set; }
        public string ProductId { get; set; }
        public int SubCategoryId { get; set; }
        public string ProductVendorId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string Brand { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int Rating { get; set; }

        // Properties from TBL_Product_Image_Details
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public byte[] Image { get; set; }
        public string ProductColor { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal? ProductMRP { get; set; }
        public bool IsAvailable { get; set; }
        public string ProductSize { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime ImageCreatedOn { get; set; }
        public DateTime ImageUpdatedOn { get; set; }
    }
}
