using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class Vendor
    {
        [Key]
        public int intVendorId { get; set; }
        public string? strName { get; set; }
        public string strUserName { get; set; }
        public string strPassword { get; set; }
        public long? longContactNo { get; set; }
        public string? strBRNo { get; set; }
        public string? strGSTNo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public string? strRejectionReason { get; set; }

        public DateTime? dtCreated_on { get; set; }
        public DateTime? dtUpdated_on { get; set; }
    }
}
