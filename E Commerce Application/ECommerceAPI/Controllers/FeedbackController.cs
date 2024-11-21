using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ECommerceAPI.Controllers.FeedbackController;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("rate")]
        public async Task<IActionResult> PostRating([FromBody] Feedback feedback)
        {
            var existingFeedback = await _context.Tbl_Feedback
                .FirstOrDefaultAsync(f => f.strProductId == feedback.strProductId && f.intUserId == feedback.intUserId);

            if (existingFeedback == null)
            {
                feedback.dtCreated_on = DateTime.UtcNow;
                feedback.dtUpdated_on = DateTime.UtcNow;
                _context.Tbl_Feedback.Add(feedback);
            }
            else
            {
                existingFeedback.intRating = feedback.intRating;
                existingFeedback.dtUpdated_on = DateTime.UtcNow;
                _context.Tbl_Feedback.Update(existingFeedback);
            }

            await _context.SaveChangesAsync();
            return Ok(existingFeedback ?? feedback);
        }

        [HttpPost("review")]
        public async Task<IActionResult> PostReview([FromBody] Feedback feedback)
        {
            var existingFeedback = await _context.Tbl_Feedback
                .FirstOrDefaultAsync(f => f.strProductId == feedback.strProductId && f.intUserId == feedback.intUserId);

            if (existingFeedback == null)
            {
                feedback.dtCreated_on = DateTime.UtcNow;
                feedback.dtUpdated_on = DateTime.UtcNow;
                _context.Tbl_Feedback.Add(feedback);
            }
            else
            {
                existingFeedback.strReview = feedback.strReview;
                existingFeedback.dtUpdated_on = DateTime.UtcNow;
                _context.Tbl_Feedback.Update(existingFeedback);
            }

            await _context.SaveChangesAsync();
            return Ok(existingFeedback ?? feedback);
        }

        [HttpGet("rate/{productId}/{userId}")]
        public async Task<IActionResult> GetRating(string productId, int userId)
        {
            var feedback = await _context.Tbl_Feedback
                .FirstOrDefaultAsync(f => f.strProductId == productId && f.intUserId == userId);

            if (feedback == null)
            {
                return NotFound("Rating not found for the specified product and user.");
            }

            return Ok(feedback);
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacksByProductId(string productId)
        {
            if (_context.Tbl_Feedback == null)
            {
                return NotFound();
            }

            var feedbacks = await _context.Tbl_Feedback
                                          .Where(f => f.strProductId == productId)
                                          .ToListAsync();

            if (feedbacks == null)
            {
                return new List<Feedback>();
            }

            return feedbacks;
        }

        [HttpGet("product/{productId}/first")]
        public async Task<ActionResult<Feedback>> GetFirstFeedbackByProductId(string productId)
        {
            if (_context.Tbl_Feedback == null)
            {
                return NotFound();
            }

            var feedback = await _context.Tbl_Feedback.Where(f => f.strProductId == productId).FirstOrDefaultAsync();

            if (feedback == null)
            {
                return NotFound();
            }

            return feedback;
        }



    }
}
