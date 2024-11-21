using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
    public class OrderedItem
    {
        public int intOrderItemId { get; set; }
        public int intOrderId { get; set; }
        public string strProductId { get; set; }
        public decimal decPrice { get; set; }
        public int intQuantity { get; set; }
        public string strSize { get; set; }
        public string Brand { get; set; }
        public string ProductName { get; set; }
        public string strOrderStatus { get; set; }
        public DateTime? dtCreated_on { get; set; }
        public DateTime? dtUpdated_on { get; set; }
    }
}
