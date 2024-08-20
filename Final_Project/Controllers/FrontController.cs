using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class FrontController : Controller
    {
        
            public IActionResult Front_Page()
            {
                return View();
            }
       
    }
}
