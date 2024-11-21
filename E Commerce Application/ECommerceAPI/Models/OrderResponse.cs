namespace ECommerceAPI.Models
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int intAddressId { get; set; }
        public int NumberOfItems { get; set; }
        public string FirstProductName { get; set; }
        public byte[]? FirstProductImage { get; set; }
        public string strPaymentMethod { get; set; }
        public DateTime dtOrderDate { get; set; }
        public decimal decTotalAmount { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; }
    }
}
