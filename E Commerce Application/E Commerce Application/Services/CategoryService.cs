using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_Application.Models;

namespace E_Commerce_Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService()
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

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Category>>("Categories");
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Category>($"Categories/{id}");
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _httpClient.PostAsJsonAsync("Categories", category);
        }

        public async Task UpdateCategoryAsync(int id, Category category)
        {
            await _httpClient.PutAsJsonAsync($"Categories/{id}", category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _httpClient.DeleteAsync($"Categories/{id}");
        }
    }
}
