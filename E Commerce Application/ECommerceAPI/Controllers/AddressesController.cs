using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AddressesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetTbl_Address()
        {
            if (_context.Tbl_Address == null)
            {
                return NotFound();
            }
            return await _context.Tbl_Address.ToListAsync();
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            if (_context.Tbl_Address == null)
            {
                return NotFound();
            }
            var address = await _context.Tbl_Address.FindAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }
        [HttpGet("byUserId/{userId}")]
        public async Task<ActionResult<Address>> GetDefaultAddressByUserId(int userId)
        {
            if (_context.Tbl_Address == null)
            {
                return NotFound();
            }

            var address = await _context.Tbl_Address.FirstOrDefaultAsync(a => a.intUserId == userId);

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }
        [HttpGet("byId/{userId}")]
        public async Task<ActionResult<List<Address>>> GetAddressByUserId(int userId)
        {
            var addresses = await _context.Tbl_Address.Where(a => a.intUserId == userId).ToListAsync();

            if (addresses == null || addresses.Count == 0)
            {
                return null;
            }

            return addresses;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, Address address)
        {
            if (id != address.intAddressId)
            {
                return BadRequest();
            }

            _context.Entry(address).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            if (_context.Tbl_Address == null)
            {
                return Problem("Entity set 'AppDbContext.Tbl_Address' is null.");
            }

            // Check if there are any existing addresses for the given user
            var existingAddresses = await _context.Tbl_Address
                                                  .Where(a => a.intUserId == address.intUserId)
                                                  .ToListAsync();

            // Set IsDefault to true if no existing addresses are found, otherwise set to false
            address.IsDefault = !existingAddresses.Any();

            _context.Tbl_Address.Add(address);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAddress", new { id = address.intAddressId }, address);
        }

        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (_context.Tbl_Address == null)
            {
                return NotFound();
            }
            var address = await _context.Tbl_Address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            _context.Tbl_Address.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AddressExists(int id)
        {
            return (_context.Tbl_Address?.Any(e => e.intAddressId == id)).GetValueOrDefault();
        }

        [HttpPut("setDefault/{id}")]
        public async Task<IActionResult> SetDefaultAddress(int id)
        {
            var address = await _context.Tbl_Address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            // Set all other addresses to not default
            var allAddresses = await _context.Tbl_Address.Where(a => a.intUserId == address.intUserId).ToListAsync();
            foreach (var addr in allAddresses)
            {
                addr.IsDefault = false;
            }

            address.IsDefault = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
