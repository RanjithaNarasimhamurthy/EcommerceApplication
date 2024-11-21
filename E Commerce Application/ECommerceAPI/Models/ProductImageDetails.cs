using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    [Table("TBL_Product_Image_Details")]
    public class ProductImageDetails
    {
        [Key]
        public int intImageId { get; set; }
        public string strProductId { get; set; }
        public string strImageName { get; set; }
        [Required]
        public byte[]? vbImage { get; set; }
        public string? strProductColor { get; set; }
        public int? intQuantityInStock { get; set; }
        public decimal? dcProductPrice { get; set; }
        public string strProductSize { get; set; }
        public DateTime? dtCreated_On { get; set; }
        public DateTime? dtUpdated_On { get; set; }

        [NotMapped]
        public List<byte[]>? Images { get; set; }
    }
}
