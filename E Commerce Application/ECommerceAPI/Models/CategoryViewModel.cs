using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Models
{
    [Keyless]
    public class CategoryViewModel
    {
        public List<Category> Categories { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
}
