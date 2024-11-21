using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class SubCategory
    {
        [Key]
        public int intSubCategoryId { get; set; }
        public int intCategoryId { get; set; }
        public string strSubCategoryName { get; set; }
        public string? strImage { get; set; }
        public DateTime dtCreated_on { get; set; }
        public DateTime dtUpdated_on { get; set; }
    }
}
