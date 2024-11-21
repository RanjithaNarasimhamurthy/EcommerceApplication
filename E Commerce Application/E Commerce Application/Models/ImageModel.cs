using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
    public class ImageModel
    {
        public byte[] ImageBytes { get; set; }
        public ImageSource ImageSource { get; set; }
        public string FileName { get; set; }
    }
}
