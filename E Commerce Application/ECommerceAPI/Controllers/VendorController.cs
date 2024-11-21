using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VendorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Vendor
        //public async Task<IActionResult> Index()
        //{
        //      return _context.Tbl_Vendor != null ? 
        //                  View(await _context.Tbl_Vendor.ToListAsync()) :
        //                  Problem("Entity set 'AppDbContext.Tbl_Vendor'  is null.");
        //}

        // GET: api/Vendor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendors()
        {
            if (_context.Tbl_Vendor == null)
            {
                return NotFound();
            }
            return await _context.Tbl_Vendor.ToListAsync();
        }

        // GET: Vendor/Details/5
        //public async Task<ActionResult<Vendor>> Details(int? id)
        //{
        //    if (id == null || _context.Tbl_Vendor == null)
        //    {
        //        return NotFound();
        //    }

        //    var vendor = await _context.Tbl_Vendor
        //        .FirstOrDefaultAsync(m => m.intVendorId == id);
        //    if (vendor == null)
        //    {
        //        return NotFound();
        //    }

        //    return vendor;
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(int id)
        {
            if (_context.Tbl_Vendor == null)
            {
                return NotFound();
            }
            var vendor = await _context.Tbl_Vendor.FindAsync(id);

            if (vendor == null)
            {
                return NotFound();
            }

            return vendor;
        }

        // GET: Vendor/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Vendor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult<Vendor>> Create([Bind("intVendorId,strName,strUserName,strPassword,longContactNo,strBRNo,strGSTNo,IsActive,IsApproved,dtCreated_on,dtUpdated_on")] Vendor vendor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(vendor);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return CreatedAtAction("GetUser", new { id = vendor.intVendorId }, vendor);
        //}

        [HttpPost]
        public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor)
        {
            if (_context.Tbl_Vendor == null)
            {
                return Problem("Entity set 'AppDbContext.User'  is null.");
            }
            vendor.dtCreated_on ??= DateTime.Now;
            vendor.dtUpdated_on ??= DateTime.Now;
            _context.Tbl_Vendor.Add(vendor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVendor", new { id = vendor.intVendorId }, vendor);
        }

        // GET: Vendor/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Tbl_Vendor == null)
        //    {
        //        return NotFound();
        //    }

        //    var vendor = await _context.Tbl_Vendor.FindAsync(id);
        //    if (vendor == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(vendor);
        //}

        // POST: Vendor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("intVendorId,strName,strUserName,strPassword,longContactNo,strBRNo,strGSTNo,IsActive,IsApproved,dtCreated_on,dtUpdated_on")] Vendor vendor)
        //{
        //    if (id != vendor.intVendorId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(vendor);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!VendorExists(vendor.intVendorId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(vendor);
        //}

        // GET: Vendor/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Tbl_Vendor == null)
        //    {
        //        return NotFound();
        //    }

        //    var vendor = await _context.Tbl_Vendor
        //        .FirstOrDefaultAsync(m => m.intVendorId == id);
        //    if (vendor == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(vendor);
        //}

        // PUT: api/Vendor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor(int id, Vendor vendor)
        {
            if (id != vendor.intVendorId)
            {
                return BadRequest();
            }

            vendor.dtUpdated_on = DateTime.Now;
            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
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

        // POST: Vendor/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Tbl_Vendor == null)
        //    {
        //        return Problem("Entity set 'AppDbContext.Tbl_Vendor'  is null.");
        //    }
        //    var vendor = await _context.Tbl_Vendor.FindAsync(id);
        //    if (vendor != null)
        //    {
        //        _context.Tbl_Vendor.Remove(vendor);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        // DELETE: api/Vendor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            if (_context.Tbl_Vendor == null)
            {
                return NotFound();
            }
            var vendor = await _context.Tbl_Vendor.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            _context.Tbl_Vendor.Remove(vendor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("{id}/Approve")]
        public async Task<IActionResult> ApproveVendor(int id)
        {
            var vendor = await _context.Tbl_Vendor.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            vendor.IsApproved = true;
            vendor.dtUpdated_on = DateTime.Now;
            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
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

        [HttpPut("{id}/Reject")]
        public async Task<IActionResult> RejectVendor(int id, Vendor updatedVendor)
        {
            var vendor = await _context.Tbl_Vendor.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            vendor.IsRejected = true;
            vendor.strRejectionReason = updatedVendor.strRejectionReason;
            vendor.dtUpdated_on = DateTime.Now;
            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
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






        private bool VendorExists(int id)
        {
          return (_context.Tbl_Vendor?.Any(e => e.intVendorId == id)).GetValueOrDefault();
        }
    }
}
