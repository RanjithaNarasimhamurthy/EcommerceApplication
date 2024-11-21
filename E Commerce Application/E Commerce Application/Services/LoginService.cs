using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace E_Commerce_Application.Services
{
    public class LoginService : ILoginService
    {
        public async Task<User> Login(string email, string password)
        {
            try
            {
                //_ = new User();
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

                var client = new HttpClient();
                string url = "http://10.0.2.2:5135/api/User/" + email + "/" + password;
                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                if (response.IsSuccessStatusCode)
                {
                    User user = await response.Content.ReadFromJsonAsync<User>();
                    if (user != null)
                    {
                        Preferences.Set("UserToken", user.strUserToken);
                        Preferences.Set("UserId", user.intUserId);

                    }
                    return await Task.FromResult(user);
                }
                return null;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                return null;
            }
            
        }
    }
}
