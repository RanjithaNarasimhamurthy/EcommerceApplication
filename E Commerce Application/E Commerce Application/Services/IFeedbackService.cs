using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Services
{
    public interface IFeedbackService
    {
        Task<Feedback> AddOrUpdateRatingAsync(Feedback feedback);
        Task<Feedback> AddOrUpdateReviewAsync(Feedback feedback);
        Task<Feedback> GetFeedbackAsync(string productId, int userId);
        Task<IEnumerable<Feedback>> GetFeedbacksByProductIdAsync(string productId, bool getFirstOnly);
    }
}
