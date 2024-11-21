using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
    public class OrderItem
    {
        public int intOrderItemId { get; set; }
        public int intOrderId { get; set; }
        public string strProductId { get; set; }
        public decimal decPrice { get; set; }
        public int intQuantity { get; set; }
        public string strSize { get; set; }
        public string strOrderStatus { get; set; }
        public decimal decItemAmount { get; set; }
        public string ProductName { get; set; }
        public byte[]? ProductImage { get; set; }
        public int? intRating { get; set; }
        public string? Name { get; set; }
        public string Brand { get; set; }
        public long? ContactNo { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? ZipCode { get; set; }
        public DateTime dtCreated_on { get; set; }
        public DateTime dtUpdated_on { get; set; }

    }
}
