using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class Product
    {
        [Key]
        public int intProductId { get; set; }
        public int intSubCategoryId { get; set; }
        public string strProductName { get; set; }
        public string strDescription { get; set; }
        public string strBrand { get; set; }
        public string strColor { get; set; }
        public string strSize { get; set; }
        public string strImageName { get; set; }
        public decimal decPrice { get; set; }

        public DateTime? dtCreated_on { get; set; }
        public DateTime? dtUpdated_on { get; set; }
    }
}
