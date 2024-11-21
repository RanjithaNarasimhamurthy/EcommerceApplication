using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync();
        Task<User> UpdateUserAsync(User user);
    }
}
