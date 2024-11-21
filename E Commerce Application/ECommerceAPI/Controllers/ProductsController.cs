using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReponseModel>>> GetTbl_Product()
        {
            var product = await _context.Products.Include(p => p.ProductImageDetails).ToListAsync();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReponseModel>> GetProduct(string id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.Include(p => p.ProductImageDetails)
                .FirstOrDefaultAsync(p => p.strProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("GetWishlist")]
        public async Task<ActionResult<List<ProductReponseModel>>> GetWishlistProduct(int userId)
        {
            var wishlistProducts = await (from wishlist in _context.Tbl_Wishlist
                                          join product in _context.Products
                                          on wishlist.strProductId equals product.strProductId
                                          where wishlist.intUserId == userId
                                          select product)
                                          .Include(p => p.ProductImageDetails)
                                          .ToListAsync();
            //return wishlistProducts ?? new List<ProductReponseModel>();
            if (wishlistProducts == null)
            {
                return new List<ProductReponseModel>();
            }
            var productIds = wishlistProducts.Select(p => p.strProductId).ToList();

            var feedbacks = await _context.Tbl_Feedback
                                          .Where(f => productIds.Contains(f.strProductId))
                                          .ToListAsync();

            var updatedProducts = wishlistProducts
                .Where(p => p.IsVisible == true)
                .Select(product =>
                {
                    var feedbackResponses = feedbacks.Where(f => f.strProductId == product.strProductId)
                                                     .Select(f => new Feedback
                                                     {
                                                         intFeedbackId = f.intFeedbackId,
                                                         intUserId = f.intUserId,
                                                         strUserName = f.strUserName,
                                                         intRating = f.intRating,
                                                         strReview = f.strReview,
                                                         dtCreated_on = f.dtCreated_on,
                                                         dtUpdated_on = f.dtUpdated_on
                                                     }).ToList();

                    return new ProductReponseModel
                    {
                        strProductId = product.strProductId,
                        intSubCategoryId = (int)product.intSubCategoryId,
                        strProductVendorId = product.strProductVendorId,
                        strProductName = product.strProductName,
                        strProductDescription = product.strProductDescription,
                        strBrand = product.strBrand,
                        dcMRP = product.dcMRP,
                        bitIsVisible = product.IsVisible,
                        bitIsDeleted = product.IsDeleted,
                        bitIsWishlist = product.IsWishlist,
                        ProductImages = product.ProductImageDetails,
                        Feedbacks = feedbackResponses,
                        dtCreated_on = product.dtCreated_On,
                        dtUpdated_on = product.dtUpdated_On
                    };
                }).ToList();

            return Ok(updatedProducts);
        }

        [HttpPut("SetWishlistTrue/{productId}/{userId}")]
        public async Task<ActionResult<ProductReponseModel>> SetWishlistProductTrue(string productId, int userId)
        {
            var product = await _context.Products
                                        .Include(p => p.ProductImageDetails)
                                        .FirstOrDefaultAsync(p => p.strProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            var wishlistEntry = await _context.Tbl_Wishlist
                                              .FirstOrDefaultAsync(w => w.strProductId == productId && w.intUserId == userId);

            if (wishlistEntry == null)
            {
                var newWishlistEntry = new Wishlist
                {
                    strProductId = productId,
                    intUserId = userId,
                    dtCreated_on = DateTime.Now
                };

                _context.Tbl_Wishlist.Add(newWishlistEntry);
            }
            else
            {
                wishlistEntry.dtUpdated_on = DateTime.Now;
                _context.Entry(wishlistEntry).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(productId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(product);
        }

        [HttpPut("SetWishlistFalse/{productId}/{userId}")]
        public async Task<ActionResult<ProductReponseModel>> SetWishlistProductFalse(string productId, int userId)
        {
            var product = await _context.Products
                                        .Include(p => p.ProductImageDetails)
                                        .FirstOrDefaultAsync(p => p.strProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            var wishlistEntry = await _context.Tbl_Wishlist
                                              .FirstOrDefaultAsync(w => w.strProductId == productId && w.intUserId == userId);

            if (wishlistEntry == null)
            {
                return NotFound();
            }

            _context.Tbl_Wishlist.Remove(wishlistEntry);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(productId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(string id, ProductReponseModel product)
        {
            if (id != product.strProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductDetails>> PostProduct(ProductDetails product)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'AppDbContext.Tbl_Product'  is null.");
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.strProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("bySubCategory/{SubcategoryId}")]
        public async Task<ActionResult<IEnumerable<ProductReponseModel>>> GetProductsBySubCategoryId(int subcategoryId)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }

            var Products = await _context.Products.Include(p => p.ProductImageDetails)
                                                  
                                               .Where(product => product.intSubCategoryId == subcategoryId)
                                               .ToListAsync();

            if (Products == null || Products.Count == 0)
            {
                return new List<ProductReponseModel>();
            }

            return Ok(Products);
        }

        private bool ProductExists(string id)
        {
            return (_context.Products?.Any(e => e.strProductId == id)).GetValueOrDefault();
        }

        [HttpGet("ProductImageFeedbackResponse/{productId}")]
        public async Task<ActionResult<ProductReponseModel>> GetProductImageFeedbackByProductId(string productId)
        {
            var product = await _context.Products.Where(p => p.IsDeleted == false).Include(p => p.ProductImageDetails)
                          .FirstOrDefaultAsync(p => p.strProductId == productId);
            if (product == null)
            {
                return NotFound();
            }

            var feedbacks = await _context.Tbl_Feedback
                                          .Where(f => f.strProductId == product.strProductId)
                                          .ToListAsync();

            var feedbackResponses = feedbacks.Select(f => new Feedback
            {
                intFeedbackId = f.intFeedbackId,
                intUserId = f.intUserId,
                strUserName = f.strUserName,
                intRating = f.intRating,
                strReview = f.strReview,
                dtCreated_on = f.dtCreated_on,
                dtUpdated_on = f.dtUpdated_on
            }).ToList();

            var productResponse = new ProductReponseModel
            {
                strProductId = product.strProductId,
                intSubCategoryId = (int)product.intSubCategoryId,
                strProductVendorId = product.strProductVendorId,
                strProductName = product.strProductName,
                strProductDescription = product.strProductDescription,
                strBrand = product.strBrand,
                dcMRP = product.dcMRP,
                bitIsVisible = product.IsVisible,
                bitIsDeleted = product.IsDeleted,
                bitIsWishlist = product.IsWishlist,
                ProductImages = product.ProductImageDetails,
                Feedbacks = feedbackResponses,
                dtCreated_on = product.dtCreated_On,
                dtUpdated_on = product.dtUpdated_On
            };

            return Ok(productResponse);
        }

        [HttpGet("AllProductImageFeedbacks")]
        public async Task<ActionResult<IEnumerable<ProductReponseModel>>> GetAllProductImageFeedbacks()
        {
            var products = await _context.Products
                                 .Where(p => p.IsDeleted == false)
                                 .Include(p => p.ProductImageDetails)
                                 .ToListAsync();

            var productIds = products.Select(p => p.strProductId).ToList();

            var feedbacks = await _context.Tbl_Feedback
                                          .Where(f => productIds.Contains(f.strProductId))
                                          .ToListAsync();

            var productImageDetails = await _context.ProductImageDetails
                                                    .Where(pid => productIds.Contains(pid.strProductId))
                                                    .ToListAsync();

            foreach (var product in products)
            {
                var productImageDetailsForProduct = productImageDetails
                    .Where(pid => pid.strProductId == product.strProductId)
                    .ToList();

                if (productImageDetailsForProduct.All(pid => pid.intQuantityInStock == 0))
                {
                    product.IsVisible = false;
                    product.dtUpdated_On = DateTime.Now;
                    _context.Entry(product).State = EntityState.Modified;
                }
            }

            await _context.SaveChangesAsync();

            var updatedProducts = products
                .Where(p => p.IsVisible == true)
                .Select(product =>
                {
                    var feedbackResponses = feedbacks.Where(f => f.strProductId == product.strProductId)
                                                     .Select(f => new Feedback
                                                     {
                                                         intFeedbackId = f.intFeedbackId,
                                                         intUserId = f.intUserId,
                                                         strUserName = f.strUserName,
                                                         intRating = f.intRating,
                                                         strReview = f.strReview,
                                                         dtCreated_on = f.dtCreated_on,
                                                         dtUpdated_on = f.dtUpdated_on
                                                     }).ToList();

                    return new ProductReponseModel
                    {
                        strProductId = product.strProductId,
                        intSubCategoryId = (int)product.intSubCategoryId,
                        strProductVendorId = product.strProductVendorId,
                        strProductName = product.strProductName,
                        strProductDescription = product.strProductDescription,
                        strBrand = product.strBrand,
                        dcMRP = product.dcMRP,
                        bitIsVisible = product.IsVisible,
                        bitIsDeleted = product.IsDeleted,
                        bitIsWishlist = product.IsWishlist,
                        ProductImages = product.ProductImageDetails,
                        Feedbacks = feedbackResponses,
                        dtCreated_on = product.dtCreated_On,
                        dtUpdated_on = product.dtUpdated_On
                    };
                }).ToList();

            return Ok(updatedProducts);
        }

        [HttpGet("AllProductImageFeedbacksBySubCategoryId/{subcategoryId}")]
        public async Task<ActionResult<IEnumerable<ProductReponseModel>>> GetAllProductImageFeedbacksBySubCategoryId(int subcategoryId)
        {
            var products = await _context.Products
                                 .Where(p => p.IsDeleted == false && p.intSubCategoryId == subcategoryId)
                                 .Include(p => p.ProductImageDetails)
                                 .ToListAsync();

            var productIds = products.Select(p => p.strProductId).ToList();

            var feedbacks = await _context.Tbl_Feedback
                                          .Where(f => productIds.Contains(f.strProductId))
                                          .ToListAsync();

            var productImageDetails = await _context.ProductImageDetails
                                                    .Where(pid => productIds.Contains(pid.strProductId))
                                                    .ToListAsync();

            foreach (var product in products)
            {
                var productImageDetailsForProduct = productImageDetails
                    .Where(pid => pid.strProductId == product.strProductId)
                    .ToList();

                if (productImageDetailsForProduct.All(pid => pid.intQuantityInStock == 0))
                {
                    product.IsVisible = false;
                    product.dtUpdated_On = DateTime.Now;
                    _context.Entry(product).State = EntityState.Modified;
                }
            }

            await _context.SaveChangesAsync();

            var updatedProducts = products
                .Where(p => p.IsVisible == true)
                .Select(product =>
                {
                    var feedbackResponses = feedbacks.Where(f => f.strProductId == product.strProductId)
                                                     .Select(f => new Feedback
                                                     {
                                                         intFeedbackId = f.intFeedbackId,
                                                         intUserId = f.intUserId,
                                                         strUserName = f.strUserName,
                                                         intRating = f.intRating,
                                                         strReview = f.strReview,
                                                         dtCreated_on = f.dtCreated_on,
                                                         dtUpdated_on = f.dtUpdated_on
                                                     }).ToList();

                    return new ProductReponseModel
                    {
                        strProductId = product.strProductId,
                        intSubCategoryId = (int)product.intSubCategoryId,
                        strProductVendorId = product.strProductVendorId,
                        strProductName = product.strProductName,
                        strProductDescription = product.strProductDescription,
                        strBrand = product.strBrand,
                        dcMRP = product.dcMRP,
                        bitIsVisible = product.IsVisible,
                        bitIsDeleted = product.IsDeleted,
                        bitIsWishlist = product.IsWishlist,
                        ProductImages = product.ProductImageDetails,
                        Feedbacks = feedbackResponses,
                        dtCreated_on = product.dtCreated_On,
                        dtUpdated_on = product.dtUpdated_On
                    };
                }).ToList();

            return Ok(updatedProducts);
        }
    }
}
