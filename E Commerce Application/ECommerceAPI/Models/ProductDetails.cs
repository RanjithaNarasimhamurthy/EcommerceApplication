using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    [Table("TBL_Product_Details")]
    public class ProductDetails
    {
        [Key]
        public string strProductId { get; set; }
        public int? intSubCategoryId { get; set; }
        public string strProductVendorId { get; set; }
        public string strProductName { get; set; }
        public string strProductDescription { get; set; }
        public decimal dcMRP { get; set; }

        //public int intQuantityInStock { get; set; }
        public string strBrand { get; set; }
        public bool IsVisible { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsWishlist { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? dtCreated_On { get; set; }
        public DateTime? dtUpdated_On { get; set; }


        //[NotMapped]
        public List<ProductImageDetails>? ProductImageDetails { get; set; }

        [NotMapped]
        public List<ProductImageDetails>? ProductImagesobj { get; set; }
        //[NotMapped]
        //public List<ProductImageDetails>? ProductImageDetails { get; set; }
        [NotMapped]
        public byte[]? vbImage { get; set; }

        

    }
}
