using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_Application.Models;
using Newtonsoft.Json;

namespace E_Commerce_Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient _httpClient;

        public AddressService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:5135/api/")
            };
        }

        public async Task<List<Address>> GetAddressByUserId()
        {
            int userId = Preferences.Get("UserId", 0);
            var response = await _httpClient.GetAsync($"Addresses/byId/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                try
                {
                    var addresses = JsonConvert.DeserializeObject<List<Address>>(content);
                    return addresses ?? new List<Address>(); 
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"JSON deserialization error: {ex.Message}");
                    Console.WriteLine($"Response content: {content}");
                    return new List<Address>(); 
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error fetching addresses: {response.StatusCode}");
                Console.WriteLine($"Error content: {errorContent}");
            }
            return new List<Address>(); 
        }



        public async Task<Address> AddAddressAsync(Address address)
        {

            var response = await _httpClient.PostAsJsonAsync("Addresses", address);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Address Added Successfully.", "OK");
                return await response.Content.ReadFromJsonAsync<Address>();
            }
            return null;
        }

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            var response = await _httpClient.GetAsync($"Addresses/{addressId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Address>();
            }
            return null;
        }
        public async Task<Address> UpdateAddressAsync(Address address)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"Addresses/{address.intAddressId}", address);

            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Address Updated Successfully.", "OK");
                await Shell.Current.GoToAsync("//AddressPage");
                return await response.Content.ReadFromJsonAsync<Address>();

            }
            else
            {
                throw new Exception("Failed to update address.");
            }
        }

        public async Task<Address> DeleteAddressAsync(int addressId)
        {
            var response = await _httpClient.DeleteAsync($"Addresses/{addressId}");
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Address Removed Successfully.", "OK");
                return null; 
            }
            return null;
        }
        public async Task SetDefaultAddressAsync(int addressId)
        {
            var response = await _httpClient.PutAsync($"Addresses/setDefault/{addressId}", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
