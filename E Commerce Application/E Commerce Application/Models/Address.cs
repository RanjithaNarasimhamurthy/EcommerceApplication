using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
   public class Address
    {
        public int intAddressId { get; set; }
        public int intUserId { get; set; }
        public string Name { get; set; }
        public long? ContactNo { get; set; }
        public string strAddressLine1 { get; set; }
        public string strAddressLine2 { get; set; }
        public string strCity { get; set; }
        public string strState { get; set; }
        public int? intZipCode { get; set; }
        public bool? IsDefault { get; set; }
        public DateTime dtCreated_on { get; set; }
        public DateTime dtUpdated_on { get;set; }
    }
}
