using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_Application.Models;

namespace E_Commerce_Application.Services
{
    public interface IAddressService
    {
        Task<List<Address>> GetAddressByUserId();
        Task<Address> UpdateAddressAsync(Address address);
    }
}
