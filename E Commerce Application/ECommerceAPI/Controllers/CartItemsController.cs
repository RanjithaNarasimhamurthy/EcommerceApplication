using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CartItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems()
        {
            if (_context.Tbl_CartItems == null)
            {
                return NotFound();
            }
            return await _context.Tbl_CartItems.ToListAsync();
        }

        [HttpGet]
        [Route("GetCartItemByProductId")]
        public async Task<ActionResult<CartItem>> GetCartItemByProductId(string productId, string productColor, string productSize, int userId)
        {
            if (string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(productColor) || string.IsNullOrEmpty(productSize))
            {
                return BadRequest("ProductId, ProductColor and ProductSize must be provided.");
            }
            
            var cartItem = await _context.Tbl_CartItems.FirstOrDefaultAsync(u => u.StrProductId == productId && u.strProductSize == productSize && u.strProductColor == productColor && u.intUserId == userId);
            //if (userId != 0)
            //{
            //    cartItem = cartItem.Where(u => u.intUserId == userId);
            //}
            //else
            //{
            //    return BadRequest("UserId must be provided.");
            //}
            if (cartItem == null)
            {
                return NotFound("Cart Item not found.");
            }

            return Ok(cartItem);
        }

        // GET: api/CartItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartItem>> GetCartItems(int id)
        {
            if (_context.Tbl_CartItems == null)
            {
                return NotFound();
            }
            var cartItems = await _context.Tbl_CartItems.FindAsync(id);

            if (cartItems == null)
            {
                return NotFound();
            }

            return cartItems;
        }

        // PUT: api/CartItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItems(int id, CartItem cartItems)
        {
            if (id != cartItems.IntCartItemId)
            {
                return BadRequest();
            }

            _context.Entry(cartItems).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartItemsExists(id))
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

        [HttpPut]
        [Route("UpdateCartItemQuantity")]
        public async Task<ActionResult<CartItem>> UpdateCartItemQuantity(CartItem cartItem)
        {
            var cartitem = await _context.Tbl_CartItems
                .FirstOrDefaultAsync(u => u.StrProductId == cartItem.StrProductId &&
                                          u.strProductColor == cartItem.strProductColor &&
                                          u.strProductSize == cartItem.strProductSize);
            if (cartitem == null)
            {
                return NotFound("Cart Item not found.");
            }
            var productImageDetails = await _context.ProductImageDetails
                                      .Where(p => p.strProductId == cartItem.StrProductId &&
                                             p.strProductColor == cartItem.strProductColor &&
                                             p.strProductSize == cartItem.strProductSize)
                                      .ToListAsync();
            if (productImageDetails == null || productImageDetails.Count == 0)
            {
                return NotFound("Product image details not found.");
            }
            if (productImageDetails.Sum(p => p.intQuantityInStock) < 1)
            {
                return BadRequest("Not enough stock available.");
            }

            foreach (var productImageDetail in productImageDetails)
            {
                if (productImageDetail.intQuantityInStock > 0)
                {
                    productImageDetail.intQuantityInStock -= 1;
                    if (productImageDetail.intQuantityInStock >= 0)
                    {
                        cartitem.IntQuantity = cartItem.IntQuantity;
                        cartItem.dtUpdated_on = DateTime.Now;
                    }
                    _context.Entry(productImageDetail).State = EntityState.Modified;
                }
            }
            cartitem.dtUpdated_on = DateTime.Now;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartItemsExists(cartitem.IntCartItemId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(cartitem);
        }

        [HttpPost]
        public async Task<ActionResult<CartItem>> PostCartItems(CartItem cartItems)
        {
            if (_context.Tbl_CartItems == null)
            {
                return Problem("Entity set 'AppDbContext.CartItems' is null.");
            }

            var productImageDetails = await _context.ProductImageDetails
                                      .Where(p => p.strProductId == cartItems.StrProductId &&
                                             p.strProductColor == cartItems.strProductColor &&
                                             p.strProductSize == cartItems.strProductSize)
                                      .ToListAsync();
            if (productImageDetails == null || productImageDetails.Count == 0)
            {
                return NotFound("Product image details not found.");
            }
            if (productImageDetails.Sum(p => p.intQuantityInStock) < 1)
            {
                return BadRequest("Not enough stock available.");
            }

            foreach (var productImageDetail in productImageDetails)
            {
                if (productImageDetail.intQuantityInStock > 0)
                {
                    productImageDetail.intQuantityInStock -= 1;
                    _context.Entry(productImageDetail).State = EntityState.Modified;
                }
            }
            cartItems.dtCreated_on = DateTime.Now;
            cartItems.dtUpdated_on = DateTime.Now;
            _context.Tbl_CartItems.Add(cartItems);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartItemsExists(cartItems.IntCartItemId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCartItems", new { id = cartItems.IntCartItemId }, cartItems);
        }

        private bool CartItemsExists(int id)
        {
            return (_context.Tbl_CartItems?.Any(e => e.IntCartItemId == id)).GetValueOrDefault();
        }
    }
}
