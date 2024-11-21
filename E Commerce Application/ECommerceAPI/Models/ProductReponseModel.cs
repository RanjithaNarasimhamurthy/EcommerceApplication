using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class ProductReponseModel
    {
        [Key]
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
        public DateTime? dtCreated_on { get; set; }
        public DateTime? dtUpdated_on { get; set; }
        public List<ProductImageDetails> ProductImages { get; set; }
        public List<Feedback> Feedbacks { get; set; }
    }
}
