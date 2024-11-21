using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:5135/api/")
            };
            
            //var token = Preferences.Get("UserToken", string.Empty);
            //if (!string.IsNullOrEmpty(token))
            //{
            //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //}
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Order>>("Orders");
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Order>($"Orders/{id}");
        }

        public async Task AddOrderAsync(Order order)
        {
            await _httpClient.PostAsJsonAsync("Orders", order);
        }

        public async Task UpdateOrderAsync(int id, Order order)
        {
            await _httpClient.PutAsJsonAsync($"Orders/{id}", order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _httpClient.DeleteAsync($"Orders/{id}");
        }

        public async Task<List<Order>> GetOrdersSummaryByUserIdAsync()
        {
            int userId = Preferences.Get("UserId", 0);
            
            return await _httpClient.GetFromJsonAsync<List<Order>>($"Orders/byUserId/{userId}");
        }
    }
}
