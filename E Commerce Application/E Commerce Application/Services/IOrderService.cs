using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(int id, Order order);
        Task DeleteOrderAsync(int id);
        Task<List<Order>> GetOrdersSummaryByUserIdAsync();
    }
}
