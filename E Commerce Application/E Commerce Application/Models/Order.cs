using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int intUserId { get; set; }
        public int intAddressId { get; set; }
        public int NumberOfItems { get; set; }
        public string FirstProductName { get; set; }
        public byte[]? FirstProductImage { get; set; }

        public string strPaymentMethod { get; set; }
        public DateTime dtOrderDate { get; set; }
        public decimal decTotalAmount { get; set; }
        public DateTime dtCreated_on { get; set; }
        public DateTime dtUpdated_on { get;  set; }
       
    }
}
