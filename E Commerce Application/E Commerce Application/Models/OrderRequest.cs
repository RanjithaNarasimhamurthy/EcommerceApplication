using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
    public class OrderRequest
    {
        public int UserId { get; set; }
        public List<OrderedItem> OrderedItems { get; set; }
    }
}
