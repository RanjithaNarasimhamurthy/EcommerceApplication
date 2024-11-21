using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
    public class Category
    {
        public int intCategoryId { get; set; }
        public string strCategoryName { get; set; }
        public string strImage { get; set; }
        public DateTime dtCreated_on { get; set; }
        public DateTime dtUpdated_on { get; set; }
    }
}
