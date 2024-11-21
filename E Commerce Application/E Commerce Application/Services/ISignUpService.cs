using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Services
{
    public interface ISignUpService
    {
        Task<User> SignUpAsync(User newUser);
        Task<Vendor> SignUpVendorAsync(Vendor newVendor);
        Task<User> GetUserByEmailAndPhoneAsync(string email, string phone);
        Task<List<Vendor>> GetVendorsAsync();
        Task<Vendor> GetVendorByIdAsync(int vendorId);
        Task<Vendor> UpdateVendorAsync(Vendor newVendor);
        Task ApproveVendorAsync(Vendor vendor);
        Task RejectVendorAsync(Vendor vendor);
    }
}
