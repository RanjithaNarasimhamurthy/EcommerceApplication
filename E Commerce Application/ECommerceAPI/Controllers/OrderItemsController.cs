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
    public class OrderItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetTbl_OrderItem()
        {
          if (_context.Tbl_OrderItem == null)
          {
              return NotFound();
          }
            return await _context.Tbl_OrderItem.ToListAsync();
        }

        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
          if (_context.Tbl_OrderItem == null)
          {
              return NotFound();
          }
            var orderItem = await _context.Tbl_OrderItem.FindAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return orderItem;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderItem orderItem)
        {
            if (id != orderItem.intOrderItemId)
            {
                return BadRequest();
            }

            _context.Entry(orderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
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
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
        {
          if (_context.Tbl_OrderItem == null)
          {
              return Problem("Entity set 'AppDbContext.Tbl_OrderItem'  is null.");
          }
            _context.Tbl_OrderItem.Add(orderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderItem", new { id = orderItem.intOrderItemId }, orderItem);
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            if (_context.Tbl_OrderItem == null)
            {
                return NotFound();
            }
            var orderItem = await _context.Tbl_OrderItem.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.Tbl_OrderItem.Remove(orderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderItemExists(int id)
        {
            return (_context.Tbl_OrderItem?.Any(e => e.intOrderItemId == id)).GetValueOrDefault();
        }

        //[HttpGet("byOrder/{orderId}")]
        ////[Authorize]
        //public async Task<ActionResult<IEnumerable<OrderItemResponse>>> GetOrderItemsByOrderId(int orderId)
        //{
        //    var orderItems = await _context.Tbl_OrderItem
        //                                   .Where(orderItem => orderItem.intOrderId == orderId)
        //                                   .ToListAsync();

        //    if (orderItems == null || orderItems.Count == 0)
        //    {
        //        return NotFound();
        //    }

        //    var productIds = orderItems.Select(oi => oi.strProductId.ToString()).ToList();
        //    var products = await _context.Products
        //                                 .Where(p => productIds.Contains(p.strProductId))
        //                                 .ToListAsync();

        //    var productImages = await _context.ProductImageDetails
        //                                      .Where(pi => productIds.Contains(pi.strProductId))
        //                                      .ToListAsync();

        //    var orderItemResponses = orderItems.Select(orderItem => {
        //        var product = products.FirstOrDefault(p => p.strProductId == orderItem.strProductId.ToString());
        //        var productImage = productImages.FirstOrDefault(pi => pi.strProductId == orderItem.strProductId.ToString());
        //        return new OrderItemResponse
        //        {
        //            intOrderItemId = orderItem.intOrderItemId,
        //            intOrderId = orderItem.intOrderId,
        //            strProductId = orderItem.strProductId,
        //            intQuantity = orderItem.intQuantity,
        //            decPrice = orderItem.decPrice,
        //            strSize = orderItem.strSize,
        //            decItemAmount = orderItem.decItemAmount,
        //            strOrderStatus = orderItem.strOrderStatus,
        //            ProductName = product?.strProductName,
        //            ProductImage = productImage?.vbImage,
        //            dtCreated_on = orderItem.dtCreated_on,
        //            dtUpdated_on = orderItem.dtUpdated_on
        //        };
        //    }).ToList();

        //    return Ok(orderItemResponses);
        //}

        [HttpGet("byOrder/{orderId}")]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<OrderItemResponse>>> GetOrderItemsByOrderId(int orderId)
        {
            var order = await _context.Tbl_Order.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }
            int userId = order.intUserId;

            var orderItems = await _context.Tbl_OrderItem
                                           .Where(orderItem => orderItem.intOrderId == orderId)
                                           .ToListAsync();

            if (orderItems == null || orderItems.Count == 0)
            {
                return NotFound();
            }

            var productIds = orderItems.Select(oi => oi.strProductId.ToString()).ToList();
            var products = await _context.Products
                                         .Where(p => productIds.Contains(p.strProductId))
                                         .ToListAsync();

            var productImages = await _context.ProductImageDetails
                                              .Where(pi => productIds.Contains(pi.strProductId))
                                              .ToListAsync();


            var feedbacks = await _context.Tbl_Feedback
                                  .Where(f => productIds.Contains(f.strProductId) && f.intUserId == userId)
                                  .ToListAsync();

            var address = await _context.Tbl_Address
                            .FirstOrDefaultAsync(a => a.intUserId == userId && a.IsDefault == true);


            var orderItemResponses = orderItems.Select(orderItem => {
                var product = products.FirstOrDefault(p => p.strProductId == orderItem.strProductId.ToString());
                var productImage = productImages.FirstOrDefault(pi => pi.strProductId == orderItem.strProductId.ToString());
                var feedback = feedbacks.FirstOrDefault(f => f.strProductId == orderItem.strProductId.ToString() && f.intUserId == userId);
                return new OrderItemResponse
                {
                    intOrderItemId = orderItem.intOrderItemId,
                    intOrderId = orderItem.intOrderId,
                    strProductId = orderItem.strProductId,
                    intQuantity = orderItem.intQuantity,
                    decPrice = orderItem.decPrice,
                    strSize = orderItem.strSize,
                    decItemAmount = orderItem.decPrice * orderItem.intQuantity,
                    strOrderStatus = orderItem.strOrderStatus,
                    ProductName = product?.strProductName,
                    Brand = product?.strBrand,
                    ProductImage = productImage?.vbImage,
                    intRating = feedback?.intRating,
                    Name = address.Name,
                    ContactNo = address.ContactNo,
                    AddressLine1 = address.strAddressLine1,
                    AddressLine2 = address?.strAddressLine2,
                    City = address.strCity,
                    State = address.strState,
                    ZipCode = address.intZipCode,
                    dtCreated_on = orderItem.dtCreated_on,
                    dtUpdated_on = orderItem.dtUpdated_on
                };
            }).ToList();

            return Ok(orderItemResponses);
        }
    }
}
