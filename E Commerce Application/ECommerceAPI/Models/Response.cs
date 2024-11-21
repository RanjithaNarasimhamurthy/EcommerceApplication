namespace ECommerceAPI.Models
{
    public class Response
    {
        public int intUserId { get; set; }
        public string? strName { get; set; }
        public string strUserName { get; set; }
        public string strPassword { get; set; }
        public long? longContactNo { get; set; }
        public string strRole { get; set; }
        public DateTime? dtCreated_on { get; set; }
        public DateTime? dtUpdated_on { get; set; }
        public string strUserMessage { get; set; }
        public string strUserToken { get; set; }
    }
}
