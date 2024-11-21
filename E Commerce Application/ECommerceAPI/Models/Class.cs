using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class CartItem
    {
        [Key]
        public int IntCartItemId { get; set; }

        public string StrProductId { get; set; }


        public int IntSubCategoryId { get; set; }
        public int intUserId { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        public string StrProductVendorId { get; set; }

        [Required]
        public int IntQuantity { get; set; }
        public string? strProductColor { get; set; }
        public string? strProductSize { get; set; }
        public decimal? dcProductPrice { get; set; }
        public DateTime dtCreated_on { get; set; }
        public DateTime dtUpdated_on { get; set; }
    }
}
