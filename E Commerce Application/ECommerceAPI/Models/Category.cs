using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class Category
    {
        [Key]
        public int intCategoryId { get; set; }
        public string strCategoryName { get; set; }
        public string? strImage { get; set; }
        public DateTime? dtCreated_on { get; set; }
        public DateTime? dtUpdated_on { get; set; }
    }
}
