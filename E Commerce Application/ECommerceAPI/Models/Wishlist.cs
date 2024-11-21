using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class Wishlist
    {
        [Key]
        public int intWishlistId { get; set; }
        public int intUserId { get; set; }
        public string strProductId { get; set; }
        public DateTime? dtCreated_on { get; set; }
        public DateTime? dtUpdated_on { get; set; }
    }
}
