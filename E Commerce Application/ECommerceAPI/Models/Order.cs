using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class Order
    {
        [Key]
        public int intOrderId { get; set; }
        public int intUserId { get; set; }
        public int intAddressId { get; set; }
        public string strPaymentMethod { get; set; }
        public DateTime dtOrderDate { get; set; }
        public decimal decTotalAmount { get; set; }
        public DateTime? dtCreated_on { get; set; }
        public DateTime? dtUpdated_on { get; set; }

    }
}
