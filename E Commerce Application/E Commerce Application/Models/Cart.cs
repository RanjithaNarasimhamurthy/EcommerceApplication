using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
    public class Cart
    {
        public int intCartId { get; set; }
        public int intUserId { get; set; }
        public decimal decTotalAmount { get; set; }
        public DateTime dtCreated_on { get; set; }
        public DateTime dtUpdated_on { get;set; }
    }
}
