using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Services
{
    public interface IOrderItemService
    {
        Task<List<OrderItem>> GetOrderItemsAsync();
        Task<OrderItem> GetOrderItemByIdAsync(int id);
        Task AddOrderItemAsync(OrderItem orderItem);
        Task UpdateOrderItemAsync(int id, OrderItem orderItem);
        Task DeleteOrderItemAsync(int id);
        Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
    }
}
