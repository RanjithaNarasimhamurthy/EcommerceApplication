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
    public class SubCategoryService : ISubCategoryService
    {
        private readonly HttpClient _httpClient;

        public SubCategoryService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:5135/api/")
            };
            var token = Preferences.Get("UserToken", string.Empty);
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<SubCategory>> GetSubCategoriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<SubCategory>>("SubCategories");
        }

        public async Task<SubCategory> GetSubCategoryByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<SubCategory>($"SubCategories/{id}");
        }

        public async Task AddSubCategoryAsync(SubCategory subcategory)
        {
            await _httpClient.PostAsJsonAsync("SubCategories", subcategory);
        }

        public async Task UpdateSubCategoryAsync(int id, SubCategory subcategory)
        {
            await _httpClient.PutAsJsonAsync($"SubCategories/{id}", subcategory);
        }

        public async Task DeleteSubCategoryAsync(int id)
        {
            await _httpClient.DeleteAsync($"SubCategories/{id}");
        }
        public async Task<List<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId)
        {
            return await _httpClient.GetFromJsonAsync<List<SubCategory>>($"SubCategories/byCategory/{categoryId}");
        }

        public async Task<int?> GetCategoryIdBySubCategoryIdAsync(int subCategoryId)
        {
            var response = await _httpClient.GetAsync($"SubCategories/categoryIdBySubCategoryId/{subCategoryId}");
            if (response.IsSuccessStatusCode)
            {
                var categoryId = await response.Content.ReadFromJsonAsync<int>();
                return categoryId;
            }
            return null;
        }
    }
}
