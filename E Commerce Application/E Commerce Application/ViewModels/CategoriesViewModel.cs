using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_Application.Models;

namespace E_Commerce_Application.ViewModels
{
    public class CategoriesViewModel
    {
        public List<Category> Categories { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
}
