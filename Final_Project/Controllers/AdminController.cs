using Final_Project.Models.Product;
using Final_Project.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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

        public IActionResult Admin_Index()
        {
            try
            {
                // Retrieve the email from the cookie
                if (Request.Cookies.ContainsKey("Email"))
                {
                    var email = Request.Cookies["Email"];
                    var user = _userRepository.GetUserByEmail(email);

                    if (user == null)
                    {
                        ViewBag.ErrorMessage = "User not found.";
                        return View("Error"); // Ensure you have an Error.cshtml view to handle errors
                    }

                    // Pass the retrieved user to the view
                    return View(user);
                }
                else
                {
                    // If no email in cookies, redirect to login or show an appropriate message
                    ViewBag.ErrorMessage = "No user is logged in.";
                    return RedirectToAction("Login", "Account"); // Adjust based on your actual login controller
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error: {ex.Message}");
                ViewBag.ErrorMessage = "An unexpected error occurred.";
                return View("Error");
            }
        }



        public IActionResult Show_User()
        {
            var users = _userRepository.GetAllUsers();

            if (users == null)
            {
                return View("Error"); // Ensure Error.cshtml exists
            }

            return View(users); // Ensure that the Show_User view is correctly set up to handle a list of users
        }

        [HttpGet]
        public IActionResult Add_Product()
        {
            return View(); // Ensure the Add_Product.cshtml view exists
        }

        [HttpPost]
        public IActionResult Add_Product(Product product)
        {
            if (ModelState.IsValid)
            {
                // Add the product to the repository
                _productRepository.AddProduct(product);

                // Return JSON response for successful addition
                return Json(new { success = true });
            }

            // Return JSON response with validation errors
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Save the file to a directory
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/media/IMAGES", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}
