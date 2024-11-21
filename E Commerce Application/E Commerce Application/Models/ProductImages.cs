using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
    public class ProductImages
    {
        public int intImageId { get; set; }
        public string strProductId { get; set; }
        public string strImageName { get; set; }
        public byte[] vbImage { get; set; }
        public string strProductSize { get; set; }
        public decimal dcProductPrice { get; set; }
        public string strProductColor { get; set; }
        public int intQuantityInStock { get; set; }
        public DateTime? dtCreated_On { get; set; }
        public DateTime? dtUpdated_On { get; set; }

        [NotMapped]
        public List<byte[]>? Images { get; set; }
    }
}
