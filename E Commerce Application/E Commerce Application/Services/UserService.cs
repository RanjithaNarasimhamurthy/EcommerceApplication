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
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService()
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

        public async Task<User> GetUserAsync()
        {
            int userId = Preferences.Get("UserId", 0);
            return await _httpClient.GetFromJsonAsync<User>($"User/{userId}");
            //var response = await _httpClient.GetAsync($"User/{userId}");
            //response.EnsureSuccessStatusCode();
            //return await response.Content.ReadFromJsonAsync<User>();
        }
        public async Task<User> UpdateUserAsync(User user)
        {
            int userId = Preferences.Get("UserId", 0);

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"User/{userId}", user);

            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Profile Updated Successfully.", "OK");
                return await response.Content.ReadFromJsonAsync<User>();

            }
            else
            {
                // Handle the error appropriately (e.g., log it, throw an exception, etc.)
                throw new Exception("Failed to update user.");
            }
        }

    }
}
