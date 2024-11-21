using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private AppDbContext _dataContext;
        public ProductController(AppDbContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public IActionResult GetProductList(string vendorId)
        {
            try
            {
                var ProductList = _dataContext.Products.Where(e => e.IsDeleted != true && e.strProductVendorId == vendorId).ToList();
                foreach (var product in ProductList)
                {

                    if (_dataContext.ProductImageDetails.Any(e => e.strProductId == product.strProductId))
                    {
                        product.ProductImageDetails = _dataContext.ProductImageDetails.Where(e => e.strProductId == product.strProductId).ToList();
                    }
                    product.vbImage = _dataContext.ProductImageDetails.Where(e => e.strProductId == product.strProductId).Select(e => e.vbImage).FirstOrDefault();

                }
                if (ProductList.Count > 0)
                {
                    return Ok(ProductList);

                }
                return BadRequest("No data Found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult GetMaxProductId(string Id)
        {
            try
            {
                string ProductId = _dataContext.Products.Where(e => e.strProductVendorId == Id).Max(e => e.strProductId);
                if (ProductId != "")
                {
                    return Ok(ProductId);
                }
                return BadRequest("No Data is found for given ID");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetCategoryList()
        {
            try
            {
                CategoryViewModel data = new CategoryViewModel();
                data.Categories = _dataContext.Tbl_Category.ToList();
                data.SubCategories = _dataContext.Tbl_SubCategory.ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //[HttpGet]
        //public IActionResult GetFiltersDataById(string productId)
        //{
        //	try
        //	{
        //		if (productId != null)
        //		{
        //			List<ProductImageDetails> productImageDetails = new List<ProductImageDetails>();
        //			productImageDetails = _dataContext.ProductImageDetails.Where(e => e.strProductId == productId)
        //		.GroupBy(t => new { t.strProductId, t.strProductColor, t.dcProductPrice, t.strProductSize, t.intQuantityInStock })
        //		.Select(g => new ProductImageDetails
        //		{
        //			strProductId = g.Key.strProductId,
        //			strProductColor = g.Key.strProductColor,
        //			dcProductPrice = g.Key.dcProductPrice,
        //			intQuantityInStock = g.Key.intQuantityInStock,
        //			strProductSize = g.Key.strProductSize
        //		}).ToList();
        //		}
        //	}
        //	catch(Exception ex)
        //	{
        //		throw new Exception(ex.Message);
        //	}
        //}

        [HttpPost]
        public IActionResult AddProductList(ProductDetails product)
        {
            try
            {
                

                var record = _dataContext.Products.Where(e => e.intSubCategoryId == product.intSubCategoryId &&
                                                            e.strProductVendorId == product.strProductVendorId &&
                                                            e.strProductName == product.strProductName &&
                                                            e.strProductDescription == product.strProductDescription &&
                                                            e.strBrand == product.strBrand
                                                            ).FirstOrDefault();
                if (record != null)
                {
                    return Conflict("Product Already Exists");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Null Object is Passed as Parameter");
                }

                if (product == null)
                {
                    return BadRequest("Null Object is passed as Parameter");
                }

                else
                {
                    _dataContext.Products.Add(product);
                    _dataContext.ProductImageDetails.AddRange(product.ProductImageDetails);
                    _dataContext.SaveChanges();
                    return Ok("Product Added Successfully!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        [HttpPost]
        public IActionResult AddSubProduct(List<ProductImageDetails> subProducts)
        {
            try
            {
                if (subProducts == null)
                {
                    return BadRequest("Null Object is Passed as Parameter");
                }
                ProductImageDetails subrecords = subProducts.FirstOrDefault();
                var record = _dataContext.ProductImageDetails.Where(e => e.strProductId == subrecords.strProductId &&
                                                                        e.dcProductPrice == subrecords.dcProductPrice &&
                                                                        e.strProductSize == subrecords.strProductSize &&
                                                                        e.strProductColor == subrecords.strProductColor &&
                                                                        e.intQuantityInStock == subrecords.intQuantityInStock
                                                                        ).FirstOrDefault();
                if (record != null)
                {
                    return Conflict("Product Already Exists");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Null Object is Passed as Parameter");
                }
                else
                {
                    _dataContext.ProductImageDetails.AddRange(subProducts);
                    _dataContext.SaveChanges();
                    return Ok("Product Added Successfully!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetProductDetailsById(string id)
        {
            try
            {
                var product = _dataContext.Products.Where(e => e.strProductId == id && e.IsDeleted != true).FirstOrDefault();
                List<ProductImageDetails> productImageDetails = _dataContext.ProductImageDetails.Where(e => e.strProductId == id).ToList();
                var GroupedProduct = _dataContext.ProductImageDetails.Where(e => e.strProductId == id)
                .GroupBy(t => new { t.strProductId, t.strProductColor, t.dcProductPrice, t.strProductSize, t.intQuantityInStock })
                .Select(g => new ProductImageDetails
                {
                    strProductId = g.Key.strProductId,
                    strProductColor = g.Key.strProductColor,
                    dcProductPrice = g.Key.dcProductPrice,
                    intQuantityInStock = g.Key.intQuantityInStock,
                    strProductSize = g.Key.strProductSize,
                    Images = g.Select(t => t.vbImage).ToList()

                }).ToList();
                product.ProductImagesobj = GroupedProduct;
                product.ProductImageDetails = productImageDetails;
                return Ok(product);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        [HttpDelete]
        public IActionResult DeleteProduct(string ProductId)
        {
            try
            {
                var product = _dataContext.Products.Where(e => e.strProductId == ProductId && e.IsDeleted != true).FirstOrDefault();
                if (product != null)
                {
                    //_dataContext.Products.Remove(product);
                    product.IsDeleted = true;
                    var productImageDetails = _dataContext.ProductImageDetails.Where(e => e.strProductId == product.strProductId).ToList();
                    if (productImageDetails != null)
                    {
                        _dataContext.ProductImageDetails.RemoveRange(productImageDetails);
                    }
                    _dataContext.SaveChanges();
                    return Ok("Product Deleted Succesfully");
                    //Alter in Images table also
                }
                else
                {
                    return BadRequest("No Data Found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
