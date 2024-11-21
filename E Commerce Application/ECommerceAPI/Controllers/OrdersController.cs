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
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetTbl_Order()
        {
          if (_context.Tbl_Order == null)
          {
              return NotFound();
          }
            return await _context.Tbl_Order.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
          if (_context.Tbl_Order == null)
          {
              return NotFound();
          }
            var order = await _context.Tbl_Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.intOrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
          if (_context.Tbl_Order == null)
          {
              return Problem("Entity set 'AppDbContext.Tbl_Order'  is null.");
          }
            _context.Tbl_Order.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.intOrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Tbl_Order == null)
            {
                return NotFound();
            }
            var order = await _context.Tbl_Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Tbl_Order.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return (_context.Tbl_Order?.Any(e => e.intOrderId == id)).GetValueOrDefault();
        }

        //[HttpGet("byUserId/{userId}")]
        //public async Task<List<OrderResponse>> GetOrdersSummaryByUserIdAsync(int userId)
        //{
        //    var ordersSummary = await _context.Tbl_Order
        //        .Where(o => o.intUserId == userId)
        //        .Select(o => new OrderResponse
        //        {
        //            OrderId = o.intOrderId,
        //            UserId = o.intUserId,
        //            NumberOfItems = o.OrderItems.Count > 0 ? o.OrderItems.Count - 1 : 0,
        //            FirstProductName = o.OrderItems.FirstOrDefault().Product.strProductName,
        //            FirstProductImage = o.OrderItems.FirstOrDefault().Product.strImageName,
        //            strPaymentMethod = o.strPaymentMethod,
        //            decTotalAmount = o.decTotalAmount,
        //            dtOrderDate = o.dtOrderDate
        //        })
        //        .ToListAsync();

        //    return ordersSummary;
        //}



        [HttpGet("byUserId/{userId}")]
        public async Task<List<OrderResponse>> GetOrdersSummaryByUserIdAsync(int userId)
        {
            var orders = await _context.Tbl_Order
                .Where(o => o.intUserId == userId)
                .ToListAsync();

            var orderResponses = new List<OrderResponse>();

            foreach (var order in orders)
            {
                var orderItems = await _context.Tbl_OrderItem
                                               .Where(oi => oi.intOrderId == order.intOrderId)
                                               .ToListAsync();

                var productIds = orderItems.Select(oi => oi.strProductId.ToString()).ToList();
                var products = await _context.Products
                                             .Where(p => productIds.Contains(p.strProductId))
                                             .ToListAsync();

                var productImages = await _context.ProductImageDetails
                                                  .Where(pi => productIds.Contains(pi.strProductId))
                                                  .ToListAsync();

                var orderItemResponses = orderItems.Select(oi => {
                    var product = products.FirstOrDefault(p => p.strProductId == oi.strProductId.ToString());
                    var productImage = productImages.FirstOrDefault(pi => pi.strProductId == oi.strProductId.ToString());
                    return new OrderItemResponse
                    {
                        intOrderItemId = oi.intOrderItemId,
                        intOrderId = oi.intOrderId,
                        strProductId = oi.strProductId,
                        intQuantity = oi.intQuantity,
                        decPrice = oi.decPrice,
                        strSize = oi.strSize,
                        decItemAmount = oi.decPrice * oi.intQuantity,
                        strOrderStatus = oi.strOrderStatus,
                        ProductName = product?.strProductName,
                        ProductImage = productImage?.vbImage,
                        dtCreated_on = oi.dtCreated_on,
                        dtUpdated_on = oi.dtUpdated_on
                    };
                }).ToList();

                var firstOrderItemResponse = orderItemResponses.FirstOrDefault();

                orderResponses.Add(new OrderResponse
                {
                    OrderId = order.intOrderId,
                    UserId = order.intUserId,
                    intAddressId = order.intAddressId,
                    NumberOfItems = orderItems.Count-1,
                    FirstProductName = firstOrderItemResponse?.ProductName,
                    FirstProductImage = firstOrderItemResponse?.ProductImage,
                    strPaymentMethod = order.strPaymentMethod,
                    decTotalAmount = order.decTotalAmount,
                    dtOrderDate = order.dtOrderDate,
                    OrderItems = orderItemResponses
                });
            }

            return orderResponses;
        }

    }
}
