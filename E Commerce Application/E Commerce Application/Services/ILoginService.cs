using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Services
{
    public interface ILoginService
    {
        Task<User> Login(string email, string password);
        //void Register(User user);
    }
}
