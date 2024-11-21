using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SubCategories

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubCategory>>> GetTbl_SubCategory()
        {
          if (_context.Tbl_SubCategory == null)
          {
              return NotFound();
          }
            return await _context.Tbl_SubCategory.ToListAsync();
        }

        // GET: api/SubCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubCategory>> GetSubCategory(int id)
        {
          if (_context.Tbl_SubCategory == null)
          {
              return NotFound();
          }
            var subCategory = await _context.Tbl_SubCategory.FindAsync(id);

            if (subCategory == null)
            {
                return NotFound();
            }

            return subCategory;
        }

        // PUT: api/SubCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubCategory(int id, SubCategory subCategory)
        {
            if (id != subCategory.intSubCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(subCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCategoryExists(id))
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

        // POST: api/SubCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubCategory>> PostSubCategory(SubCategory subCategory)
        {
          if (_context.Tbl_SubCategory == null)
          {
              return Problem("Entity set 'AppDbContext.Tbl_SubCategory'  is null.");
          }
            _context.Tbl_SubCategory.Add(subCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubCategory", new { id = subCategory.intSubCategoryId }, subCategory);
        }

        // DELETE: api/SubCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            if (_context.Tbl_SubCategory == null)
            {
                return NotFound();
            }
            var subCategory = await _context.Tbl_SubCategory.FindAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }

            _context.Tbl_SubCategory.Remove(subCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubCategoryExists(int id)
        {
            return (_context.Tbl_SubCategory?.Any(e => e.intSubCategoryId == id)).GetValueOrDefault();
        }

        [HttpGet("byCategory/{categoryId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SubCategory>>> GetSubCategoriesByCategoryId(int categoryId)
        {
            if (_context.Tbl_SubCategory == null)
            {
                return NotFound();
            }

            var subCategories = await _context.Tbl_SubCategory
                                               .Where(subCategory => subCategory.intCategoryId == categoryId)
                                               .ToListAsync();

            if (subCategories == null || subCategories.Count == 0)
            {
                return NotFound();
            }

            return Ok(subCategories);
        }

        [HttpGet("categoryIdBySubCategoryId/{subCategoryId}")]
        public async Task<ActionResult<int>> GetCategoryIdBySubCategoryId(int subCategoryId)
        {
            if (_context.Tbl_SubCategory == null)
            {
                return NotFound();
            }

            var subCategory = await _context.Tbl_SubCategory.FindAsync(subCategoryId);

            if (subCategory == null)
            {
                return NotFound();
            }

            return Ok(subCategory.intCategoryId);
        }

    }
}
