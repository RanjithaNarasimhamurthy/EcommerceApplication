using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
    public class CombinedProductDetailsResponse
    {
        [JsonProperty("result")]
        public object Result { get; set; }

        [JsonProperty("value")]
        public List<CartResponseModel> Value { get; set; }
    }
}
