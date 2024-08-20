using Final_Project.Models.Product;
using Final_Project.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public AdminController(IUserRepository userRepository, IProductRepository productRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public IActionResult Show_User()
        {
            var users = _userRepository.GetAllUsers(); // Assuming GetAllUsers is a method in IUserRepository
            ViewBag.Users = users;
            ViewBag.TotalUsers = users.Count; // Calculate the total number of users
            return View();
        }

        [HttpGet]
        public IActionResult Add_Product()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add_Product(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.AddProduct(product); // Assuming AddProduct is a method in IProductRepository
                return RedirectToAction("Admin_Index", "Home"); // Redirect to admin index or another relevant action
            }

            return View(product);
        }
    }
}
