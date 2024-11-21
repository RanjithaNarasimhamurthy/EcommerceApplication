using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Services
{
    public interface ICartItemsService
    {
        Task<bool> AddToCartAsync(CartItem cartItem);
        Task<CartItem> GetCartItemsAsync(string productId, string productColor, string productSize, int userId);
        Task<bool> UpdateCartItemQuantityAsync(CartItem cartItem);
    }
}
