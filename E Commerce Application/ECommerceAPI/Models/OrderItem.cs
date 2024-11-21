using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace ECommerceAPI.Models
{
    public class OrderItem
    {
        [Key]
        public int intOrderItemId { get; set; }
        public int intOrderId { get; set; }
        public string strProductId { get; set; }
        public decimal decPrice { get; set; }
        public int intQuantity { get; set; }
        public string strSize { get; set; }
        public string strOrderStatus { get; set; }
        //public decimal? decItemAmount { get; set; }
        public DateTime? dtCreated_on { get; set; }
        public DateTime? dtUpdated_on { get; set; }

    }
}
