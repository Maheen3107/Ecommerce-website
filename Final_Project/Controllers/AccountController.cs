using Final_Project.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (Request.Cookies.ContainsKey("Email"))
            {
                var email = Request.Cookies["Email"];
                return View(new LoginModel { Email = email });
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetUserByEmailAndPassword(model.Email, model.Password);

                if (user == null)
                {
                    ViewData["InvalidLogin"] = "Invalid email or password.";
                }
                else
                {
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                        HttpOnly = true,
                        Secure = HttpContext.Request.IsHttps
                    };
                    Response.Cookies.Append("Email", model.Email, cookieOptions);

                    HttpContext.Session.SetString("Email", user.Email);

                    return RedirectToAction("Shop", "Shop");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Signin()
        {
            return View(new User());
        }

        [HttpPost]
        public IActionResult Signin(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _userRepository.GetUserByEmail(user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "This email is already registered.");
                }
                else
                {
                    bool isRegistered = _userRepository.RegisterUser(user);
                    if (isRegistered)
                    {
                        var cookieOptions = new CookieOptions
                        {
                            Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                            HttpOnly = true,
                            Secure = HttpContext.Request.IsHttps
                        };

                        // Store all user information in session (avoid storing sensitive info like password in cookies)
                        HttpContext.Session.SetString("Email", user.Email);
                        HttpContext.Session.SetString("Name", user.Name);
                        HttpContext.Session.SetString("Address", user.Address);
                        HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber);
                        HttpContext.Session.SetString("City", user.City);
                        HttpContext.Session.SetString("State", user.State);
                        HttpContext.Session.SetString("PostalCode", user.PostalCode);
                        HttpContext.Session.SetString("Country", user.Country);

                        return RedirectToAction("Login", "Account");
                    }
                }
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("Email"))
            {
                Response.Cookies.Delete("Email");
            }

            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
