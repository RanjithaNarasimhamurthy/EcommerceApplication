using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Services
{
    public class SignUpService : ISignUpService
    {
        private readonly HttpClient _httpClient;
        //private readonly AppDbContext _context;

        public SignUpService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:5135/api/")
            };
        }
        public async Task<User> SignUpAsync(User newUser)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("User", newUser);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<User>();
                }
                else
                {
                    throw new Exception("Signup failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception($"An error occurred during signup: {ex.Message}");
            }
        }

        //public async Task<User> GetUserByEmailAndPhoneAsync(string email, long phone)
        //{
        //    return await _context.Tbl_User.FirstOrDefaultAsync(u => u.strUserName == email && u.intContactNo == phone);
        //}

        //public async Task<User> GetUserByEmailAndPhoneAsync(string email, string phone)
        //{
        //    try
        //    {
        //        System.Net.ServicePointManager.ServerCertificateValidationCallback +=
        //        (sender, certificate, chain, sslPolicyErrors) => true;

        //        var client = new HttpClient();
        //        string url = "http://10.0.2.2:5135/api/User/" + email + "/" + phone;
        //        client.BaseAddress = new Uri(url);
        //        HttpResponseMessage response = await _httpClient.GetAsync(client.BaseAddress);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            User user = await response.Content.ReadFromJsonAsync<User>();
        //            return await Task.FromResult(user);
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        //        return null;
        //    }
        //}

        public async Task<Vendor> SignUpVendorAsync(Vendor newVendor)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Vendor", newVendor);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Vendor>();
                }
                else
                {
                    throw new Exception("Vendor signup failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during vendor signup: {ex.Message}");
            }
        }

        public async Task<User> GetUserByEmailAndPhoneAsync(string email, string phone)
        {
            try
            {
                var url = $"User/GetUserByEmailAndPhone?email={email}&phone={phone}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    User user = await response.Content.ReadFromJsonAsync<User>();
                    return user;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null; // User not found
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

        public async Task<Vendor> GetVendorByIdAsync(int vendorId)
        {
            try
            {
                var url = $"Vendor/{vendorId}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Vendor>();
                }
                else
                {
                    throw new Exception("Error fetching user by ID.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching user by ID: {ex.Message}");
            }
        }

        public async Task<Vendor> UpdateVendorAsync(Vendor vendor)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"Vendor/{vendor.intVendorId}", vendor);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Vendor>();
                }
                else
                {
                    throw new Exception("Failed to update user information. Please try again.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating user information: {ex.Message}");
            }
        }

        public async Task<List<Vendor>> GetVendorsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Vendor");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Vendor>>();
                }
                else
                {
                    throw new Exception("Failed to fetch vendors. Please try again.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching vendors: {ex.Message}");
            }
        }

        public async Task ApproveVendorAsync(Vendor vendor)
        {
            try
            {
                vendor.IsApproved = true;
                var response = await _httpClient.PutAsJsonAsync($"Vendor/{vendor.intVendorId}/Approve", vendor);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to approve vendor. Please try again.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while approving vendor: {ex.Message}");
            }
        }

        public async Task RejectVendorAsync(Vendor vendor)
        {
            try
            {
                vendor.IsApproved = false;
                var response = await _httpClient.PutAsJsonAsync($"Vendor/{vendor.intVendorId}/Reject", vendor);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to reject vendor. Please try again.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while rejecting vendor: {ex.Message}");
            }
        }

    }
}
