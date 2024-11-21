using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class ProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        // Existing methods...

        public async Task<IEnumerable<ProductDetails>> GetAllAsync()
        {
            return await _context.Products.Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<ProductDetails> GetByIdAsync(string id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.strProductId == id && !p.IsDeleted);
        }

        public async Task AddAsync(ProductDetails product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductDetails product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int CartItemId)
        {
            var cartItem = await _context.Tbl_CartItems.FirstOrDefaultAsync(c => c.IntCartItemId == CartItemId);
            if (cartItem != null)
            {
                _context.Tbl_CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }




        public async Task ClearAllAsync(int userId)
        {
            var products = await _context.Tbl_CartItems.Where(a => a.intUserId == userId).ToListAsync();


            foreach (var product in products)
            {
                _context.Tbl_CartItems.Remove(product);
            }


            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartItemAsync(int userId, string productId)
        {
            var cartItem = await _context.Tbl_CartItems
                .FirstOrDefaultAsync(ci => ci.intUserId == userId && ci.StrProductId == productId);

            if (cartItem != null)
            {
                _context.Tbl_CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartItemsForUserAsync(int userId)
        {
            var cartItems = await _context.Tbl_CartItems.Where(ci => ci.intUserId == userId).ToListAsync();

            if (cartItems.Any())
            {
                _context.Tbl_CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
        }


        //public async Task ClearAllAsync(int userId)
        //{
        //    var products = await _context.Tbl_CartItems.Where(a=> a.intUserId==userId).ToListAsync();
        //    foreach (var product in products)
        //    {
        //        product.IsDeleted = true;
        //    }
        //    await _context.SaveChangesAsync();
        //}

        // Ordered Items operations

        public async Task AddOrderedItemAsync(OrderItem orderedItem)
        {
            await _context.Tbl_OrderItem.AddAsync(orderedItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetAllOrderedItemsAsync()
        {
            return await _context.Tbl_OrderItem.ToListAsync();
        }

        public async Task<OrderItem> GetOrderedItemByIdAsync(int id)
        {
            return await _context.Tbl_OrderItem.FirstOrDefaultAsync(o => o.intOrderItemId == id);
        }

        public async Task<int> GetLatestOrderId()
        {
            int orderID = await _context.Tbl_Order.MaxAsync(o => o.intOrderId);
            return orderID;
        }

        public async Task DeleteOrderedItemAsync(string id)
        {
            var orderedItem = await _context.Tbl_OrderItem.FindAsync(id);
            if (orderedItem != null)
            {
                _context.Tbl_OrderItem.Remove(orderedItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddOrderItemAsync(Order order)
        {
            await _context.Tbl_Order.AddAsync(order);
            await _context.SaveChangesAsync();
        }


        // Rating 

        public async Task<int> GetAvgRatingByProductIdAsync(string productId)
        {
            var rating = await _context.Tbl_Feedback.Where(o => o.strProductId == productId).AverageAsync(a => a.intRating);
            return (int)rating;
        }
        // Addresses operations
        public async Task<IEnumerable<Address>> GetAddressesByUserIdAsync(int userId)
        {
            return await _context.Tbl_Address
                .Where(a => a.intUserId == userId)
                .ToListAsync();
        }

        [HttpGet("combinedProductDetails")]
        public async Task<ActionResult<IEnumerable<CartResponseModel>>> GetCombinedProductDetails(int userid)
        {
            // Step 1: Fetch and filter cart details by userid
            var cartDetails = await _context.Tbl_CartItems
                .Where(items => items.intUserId == userid)
                .Select(items => new
                {
                    items.IntCartItemId,
                    items.StrProductId,
                    items.IntSubCategoryId,
                    items.IntQuantity,
                    items.intUserId,
                    items.strProductColor,
                    items.strProductSize,
                    items.dcProductPrice
                })
                .ToListAsync();

            // Extract the product IDs from the cart details
            var productIds = cartDetails.Select(c => c.StrProductId).Distinct().ToList();

            // Step 2: Fetch product details matching the product IDs from the cart
            var productDetails = await _context.Products
                .Where(product => productIds.Contains(product.strProductId))
                .Select(product => new
                {
                    product.strProductId,
                    product.intSubCategoryId,
                    product.strProductVendorId,
                    product.strProductName,
                    product.strProductDescription,
                    product.strBrand,
                    product.dcMRP,
                    product.dtCreated_On,
                    product.dtUpdated_On,
                    product.IsAvailable
                })
                .ToListAsync();

            // Step 3: Fetch image details for the matching products
            var imageDetails = await _context.ProductImageDetails
                .Where(image => productIds.Contains(image.strProductId))
                .Select(image => new
                {

                    image.strProductColor,
                    image.strProductSize,
                    image.intImageId,
                    image.strProductId,
                    image.strImageName,
                    image.vbImage,
                    image.dtCreated_On,
                    image.dtUpdated_On
                })
                .ToListAsync();

            // Step 4: Fetch average ratings using foreach loop
            var httpClient = new HttpClient();
            var ratings = new Dictionary<string, int?>();

            foreach (var productId in productIds)
            {
                var response = await httpClient.GetAsync($"http://localhost:5135/api/Cart/GetAvgRatingByProductId/GetAvgRatingByProductId/{productId}");
                if (response.IsSuccessStatusCode)
                {
                    var ratingString = await response.Content.ReadAsStringAsync();
                    ratings[productId] = int.Parse(ratingString);
                }
                else
                {
                    ratings[productId] = null;
                }
            }

            // Step 5: Combine product, image, and cart details into CartResponseModel
            var combinedProductDetails = cartDetails.Select(cartItem =>
            {
                var product = productDetails.FirstOrDefault(p => p.strProductId == cartItem.StrProductId);
                var image = imageDetails.FirstOrDefault(img => img.strProductId == cartItem.StrProductId && img.strProductColor == cartItem.strProductColor && img.strProductSize == cartItem.strProductSize);
                var rating = ratings.ContainsKey(cartItem.StrProductId) ? ratings[cartItem.StrProductId] : 0;

                return new CartResponseModel
                {
                    IntCartItemId = cartItem.IntCartItemId,
                    ProductId = cartItem.StrProductId,
                    SubCategoryId = cartItem.IntSubCategoryId,
                    ProductVendorId = product?.strProductVendorId,
                    ProductName = product?.strProductName,
                    ProductDescription = product?.strProductDescription,
                    Brand = product?.strBrand,
                    CreatedOn = product?.dtCreated_On ?? DateTime.MinValue,
                    UpdatedOn = product?.dtUpdated_On ?? DateTime.MinValue,
                    Rating = rating ?? 0,
                    IsAvailable = product?.IsAvailable ?? false,
                    ImageId = image?.intImageId ?? 0,
                    ImageName = image?.strImageName,
                    Image = image?.vbImage,
                    ProductColor = cartItem.strProductColor,
                    ProductPrice = cartItem.dcProductPrice ?? 0,
                    ProductMRP = product?.dcMRP ?? 0,
                    ProductSize = cartItem.strProductSize,
                    QuantityInStock = cartItem.IntQuantity,
                    ImageCreatedOn = image?.dtCreated_On ?? DateTime.MinValue,
                    ImageUpdatedOn = image?.dtUpdated_On ?? DateTime.MinValue
                };
            }).ToList();

            return combinedProductDetails;
        }




    }
}
