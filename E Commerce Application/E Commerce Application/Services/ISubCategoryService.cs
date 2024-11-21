using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Services
{
    public interface ISubCategoryService
    {
        Task<List<SubCategory>> GetSubCategoriesAsync();
        Task<SubCategory> GetSubCategoryByIdAsync(int id);
        Task AddSubCategoryAsync(SubCategory subcategory);
        Task UpdateSubCategoryAsync(int id, SubCategory subcategory);
        Task DeleteSubCategoryAsync(int id);
        Task<List<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId);
        Task<int?> GetCategoryIdBySubCategoryIdAsync(int subCategoryId);
    }
}
