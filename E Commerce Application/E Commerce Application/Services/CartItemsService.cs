using E_Commerce_Application.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Services
{
    public partial class CartItemsService : ICartItemsService
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://10.0.2.2:5135/api/")
        };
        public async Task<bool> AddToCartAsync(CartItem cartItem)
        {
            try
            {
                var json = JsonConvert.SerializeObject(cartItem);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("CartItems", content);

                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Failed to add to cart: {response.StatusCode}, {responseContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while adding to cart: {ex.Message}");
                return false;
            }
        }

        public async Task<CartItem> GetCartItemsAsync(string productId, string productColor, string productSize, int userId)
        {
            try
            {
                var url = $"CartItems/GetCartItemByProductId?productId={productId}&productColor={productColor}&productSize={productSize}&userId={userId}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    CartItem cartItem = await response.Content.ReadFromJsonAsync<CartItem>();
                    return cartItem;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching user information: {ex.Message}");
            }
        }

        public async Task<bool> UpdateCartItemQuantityAsync(CartItem cartItem)
        {
            var response = await _httpClient.PutAsJsonAsync("CartItems/UpdateCartItemQuantity", cartItem);
            return response.IsSuccessStatusCode;
        }

    }
}
