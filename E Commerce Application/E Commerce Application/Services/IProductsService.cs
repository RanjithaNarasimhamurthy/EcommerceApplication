using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_Application.Models;

namespace E_Commerce_Application.Services
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductResponse>> GetProductsAsync();
        Task<ProductResponse> GetProductByIdAsync(string id);
        Task<IEnumerable<ProductResponse>> GetWishlistProductsAsync(int userId);
        Task<ProductResponse> SetWishlistProductTrueByIdAsync(string id);
        Task<ProductResponse> SetWishlistProductFalseByIdAsync(string id);
        Task<List<ProductResponse>> GetProductsBySubCategoryId(int subCategoryId);
    }
}
