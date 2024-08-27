using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;  // Ensure this namespace is included for JSON serialization
using Final_Project.Models.User;  // Ensure this namespace is correct

namespace Final_Project.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserRepository _userRepository;

        public ProfileController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Profile()
        {
            try
            {
                // Retrieve the email from the cookie
                if (Request.Cookies.ContainsKey("Email"))
                {
                    var email = Request.Cookies["Email"];

                    // Fetch the user from the repository using the email
                    var user = _userRepository.GetUserByEmail(email);

                    if (user == null)
                    {
                        ViewBag.ErrorMessage = "User not found.";
                        return View("Error"); // Ensure you have an Error.cshtml view to handle errors
                    }

                    // Pass the retrieved user to the profile view
                    return View(user);
                }
                else
                {
                    // If no email in cookies, redirect to the login page
                    TempData["ErrorMessage"] = "Please log in to view your profile.";
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
    }
}
