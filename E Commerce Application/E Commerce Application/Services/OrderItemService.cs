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
    public class OrderItemService : IOrderItemService
    {
        private readonly HttpClient _httpClient;

        public OrderItemService()
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

        public async Task<List<OrderItem>> GetOrderItemsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<OrderItem>>("OrderItems");
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<OrderItem>($"OrderItems/{id}");
        }

        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            await _httpClient.PostAsJsonAsync("OrderItems", orderItem);
        }

        public async Task UpdateOrderItemAsync(int id, OrderItem orderItem)
        {
            await _httpClient.PutAsJsonAsync($"OrderItems/{id}", orderItem);
        }

        public async Task DeleteOrderItemAsync(int id)
        {
            await _httpClient.DeleteAsync($"OrderItems/{id}");
        }
        public async Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await _httpClient.GetFromJsonAsync<List<OrderItem>>($"OrderItems/byOrder/{orderId}");
        }
    }
}
