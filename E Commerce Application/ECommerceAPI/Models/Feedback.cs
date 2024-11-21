using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class Feedback
    {
        [Key]
        public int intFeedbackId { get; set; }
        public int intUserId { get; set; }
        public string? strUserName { get; set; }
        public string strProductId { get; set; }
        public int? intRating { get; set; }
        public string? strReview { get; set; }
        public DateTime? dtCreated_on { get; set; }
        public DateTime? dtUpdated_on { get; set; }
    }
}
