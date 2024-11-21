using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Models
{
   public class User
    {
        public int intUserId { get; set; }
        public string strName { get; set; }
        public string strUserName { get; set; }
        public string strPassword { get; set; }
        public long? longContactNo { get; set; }
        public string strRole { get; set; }
        public DateTime dtCreated_on { get; set; }
        public DateTime dtUpdated_on { get; set;}
        public string strUserToken { get; set; }
    }
}
