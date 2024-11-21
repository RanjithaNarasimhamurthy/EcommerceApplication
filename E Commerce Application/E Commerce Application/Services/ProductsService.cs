using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_Application.Models;

namespace E_Commerce_Application.Services
{
    public class ProductsService : IProductsService
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://10.0.2.2:5135/api/")
        };
        private static readonly Dictionary<string, ProductResponse> ProductCache = new Dictionary<string, ProductResponse>();
        private static readonly List<ProductResponse> ProductsCache = new List<ProductResponse>();

        public async Task<ProductResponse> GetProductByIdAsync(string id)
        {
            if (ProductCache.TryGetValue(id, out var cachedProduct))
            {
                return cachedProduct;
            }

            var response = await _httpClient.GetFromJsonAsync<ProductResponse>($"Products/ProductImageFeedbackResponse/{id}");
            ProductCache[id] = response ?? throw new Exception("Product not found.");
            return response;
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsAsync()
        {
            if (ProductsCache.Any())
            {
                return ProductsCache;
            }

            var response = await _httpClient.GetFromJsonAsync<List<ProductResponse>>("Products/AllProductImageFeedbacks");
            if (response != null)
            {
                ProductsCache.AddRange(response);
            }
            return response ?? throw new Exception("Products not found.");
        }

        public async Task<ProductResponse> SetWishlistProductFalseByIdAsync(string id) =>
            await UpdateWishlistStatus(id, "SetWishlistFalse");

        public async Task<ProductResponse> SetWishlistProductTrueByIdAsync(string id) =>
            await UpdateWishlistStatus(id, "SetWishlistTrue");

        private async Task<ProductResponse> UpdateWishlistStatus(string id, string action)
        {
            int userId = Preferences.Get("UserId", 0);
            var response = await _httpClient.PutAsJsonAsync($"Products/{action}/{id}/{userId}", new { });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductResponse>();
        }

        public async Task<IEnumerable<ProductResponse>> GetWishlistProductsAsync(int userId) =>
       await _httpClient.GetFromJsonAsync<List<ProductResponse>>($"Products/GetWishlist?userId={userId}");


        public async Task<List<ProductResponse>> GetProductsBySubCategoryId(int subCategoryId)
        {
            return await _httpClient.GetFromJsonAsync<List<ProductResponse>>($"Products/AllProductImageFeedbacksBySubCategoryId/{subCategoryId}");
        }


    }

}

