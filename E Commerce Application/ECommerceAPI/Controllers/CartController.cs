using ECommerceAPI.Models;
using ECommerceAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CartController : ControllerBase
    {
        private readonly ProductRepository _repository;

        public CartController(ProductRepository repository)
        {
            _repository = repository;
        }

        // Existing methods...

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDetails>>> Get()
        {
            var products = await _repository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetails>> Get(string id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<int>> GetLatestOrderId()
        {
            var Id = await _repository.GetLatestOrderId();
            if (Id == 0)
            {
                return NotFound();
            }
            return Ok(Id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDetails product)
        {
            await _repository.AddAsync(product);
            return CreatedAtAction(nameof(Get), new { id = product.strProductId }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ProductDetails product)
        {
            if (id != product.strProductId)
            {
                return BadRequest();
            }

            var existingProduct = await _repository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            await _repository.UpdateAsync(product);
            return NoContent();
        }

        [HttpDelete("{CartItemId}")]
        public async Task<ActionResult> Delete(int CartItemId)
        {

            await _repository.DeleteAsync(CartItemId);
            return NoContent();
        }

        [HttpDelete("ClearAll/{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            await _repository.ClearAllAsync(userId);
            return NoContent();
        }

        //[HttpPost("PlaceOrder")]
        //public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest orderRequest)
        //{
        //    if (orderRequest == null || orderRequest.OrderedItems == null || orderRequest.OrderedItems.Count == 0)
        //    {
        //        return BadRequest("Ordered items cannot be empty.");
        //    }

        //    try
        //    {
        //        foreach (var item in orderRequest.OrderedItems)
        //        {
        //            await _repository.AddOrderedItemAsync(item);
        //        }

        //        foreach (var item in orderRequest.OrderedItems)
        //        {
        //            await _repository.DeleteCartItemAsync(orderRequest.UserId, item.strProductId);
        //        }

        //        return Ok("Order placed successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (ex) if necessary
        //        return StatusCode(500, "Failed to place order. Please try again later.");
        //    }
        //}



        [HttpPost("PlaceOrder/{userID}")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest orderRequest)
        {
            if (orderRequest == null || orderRequest.OrderedItems == null || orderRequest.OrderedItems.Count == 0)
            {
                return BadRequest("Ordered items cannot be empty.");
            }

            try
            {
                foreach (var item in orderRequest.OrderedItems)
                {
                    await _repository.AddOrderedItemAsync(item);
                }

                foreach (var item in orderRequest.OrderedItems)
                {
                    await _repository.ClearCartItemsForUserAsync(orderRequest.UserId);
                }

                return Ok("Order placed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception (ex) if necessary
                return StatusCode(500, "Failed to place order. Please try again later.");
            }
        }



        [HttpPost("ProceedToOrder")]
        public async Task<IActionResult> ProceedToOrder([FromBody] Order order)
        {
            try
            {
                await _repository.AddOrderItemAsync(order);
                return Ok("Order placed successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to place order. Please try again later.");
            }
        }

        [HttpGet("GetCombinedProductDetails/{id}")]
        public async Task<ActionResult<IEnumerable<CartResponseModel>>> GetCombinedProductDetails(int id = 101)
        {
            var combinedProductDetails = await _repository.GetCombinedProductDetails(id);
            return Ok(combinedProductDetails);
        }

        [HttpGet("GetAddressesByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddressesByUserId(int userId = 1)
        {
            var addresses = await _repository.GetAddressesByUserIdAsync(userId);
            if (addresses == null || !addresses.Any())
            {
                return NotFound();
            }
            return Ok(addresses);
        }

        [HttpGet("GetAvgRatingByProductId/{productId}")]

        public async Task<ActionResult> GetAvgRatingByProductId(string productId)
        {
            var rating = await _repository.GetAvgRatingByProductIdAsync(productId);
            if (rating == null)
            {
                return NotFound();
            }
            return Ok(rating);
        }

        [HttpGet("GetOrderItemById/{id}")]
        public async Task<ActionResult> GetOrderItemById(int id)
        {
            var ORderItem = await _repository.GetOrderedItemByIdAsync(id);
            if (ORderItem == null)
            {
                return BadRequest();
            }
            return Ok(ORderItem);
        }
    }
}
