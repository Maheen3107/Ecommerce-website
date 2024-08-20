using Microsoft.AspNetCore.Mvc;
using Final_Project.Models; // Adjust based on your project structure
using Final_Project.Models.Product;
namespace Final_Project.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IProductRepository _productRepository;

        public CategoryController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> CategoryProducts(int categoryId)
        {
            if (categoryId <= 0)
            {
                return BadRequest("Invalid category ID");
            }

            // Fetch products by category ID
            var products = await _productRepository.GetProductsByCategory(categoryId);

            // If no products found, return a NotFound result
            if (products == null || !products.Any())
            {
                return NotFound("No products found for this category");
            }

            // Pass the products to the view
            return View(products);
        }
        public IActionResult About_Us()
        {
            return View();
        }
    }
}
