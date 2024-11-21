using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly HttpClient _httpClient;

        public FeedbackService()
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
        public async Task<Feedback> AddOrUpdateRatingAsync(Feedback feedback)
        {
            var response = await _httpClient.PostAsJsonAsync($"Feedback/rate", feedback);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Feedback>();
        }

        public async Task<Feedback> AddOrUpdateReviewAsync(Feedback feedback)
        {
            var response = await _httpClient.PostAsJsonAsync($"Feedback/review", feedback);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Feedback>();
        }

        public async Task<Feedback> GetFeedbackAsync(string productId, int userId)
        {
            var response = await _httpClient.GetAsync($"Feedback/rate/{productId}/{userId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Feedback>();
        }


        public async Task<IEnumerable<Feedback>> GetFeedbacksByProductIdAsync(string productId, bool getFirstOnly = false)
        {
            try
            {
                var url = getFirstOnly ? $"Feedback/product/{productId}/first" : $"Feedback/product/{productId}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    if (getFirstOnly)
                    {
                        var feedback = await response.Content.ReadFromJsonAsync<Feedback>();
                        if (feedback != null)
                        {
                            var userResponse = await _httpClient.GetAsync($"User/{feedback.intUserId}/details");
                            if (userResponse.IsSuccessStatusCode)
                            {
                                var user = await userResponse.Content.ReadFromJsonAsync<User>();
                                feedback.strUserName = user?.strName ?? "Unknown User";
                            }
                            else
                            {
                                feedback.strUserName = "Unknown User";
                            }
                            return new List<Feedback> { feedback };
                        }
                        else
                        {
                            return new List<Feedback>();
                        }
                    }
                    else
                    {
                        var feedbacks = await response.Content.ReadFromJsonAsync<IEnumerable<Feedback>>() ?? new List<Feedback>();

                        foreach (var feedback in feedbacks)
                        {
                            var userResponse = await _httpClient.GetAsync($"User/{feedback.intUserId}/details");
                            if (userResponse.IsSuccessStatusCode)
                            {
                                var user = await userResponse.Content.ReadFromJsonAsync<User>();
                                feedback.strUserName = user?.strName ?? "Unknown User";
                            }
                            else
                            {
                                feedback.strUserName = "Unknown User";
                            }
                        }

                        return feedbacks;
                    }
                }
                else
                {
                    return new List<Feedback>();
                }
            }
            catch (Exception)
            {
                return new List<Feedback>();
            }
        }

    }
}
