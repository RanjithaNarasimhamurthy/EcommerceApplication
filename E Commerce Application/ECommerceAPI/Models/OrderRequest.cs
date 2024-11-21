namespace ECommerceAPI.Models
{
    public class OrderRequest
    {
        public int UserId { get; set; }
        public List<OrderItem> OrderedItems { get; set; }
    }
}
