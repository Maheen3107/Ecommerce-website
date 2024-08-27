using Microsoft.AspNetCore.Mvc;
using Final_Project.Models.Cart;
using Final_Project.Helper;
namespace Final_Project.Controllers
{
    public class CartController : Controller
    {
  
            public IActionResult AddToCart(int ProductId, string ProductName, string ImageUrl, decimal Price)
            {
                List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

                var existingItem = cart.Find(c => c.ProductId == ProductId);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    cart.Add(new CartItem
                    {
                        ProductId = ProductId,
                        ProductName = ProductName,
                        ImageUrl = ImageUrl,
                        Price = Price,
                        Quantity = 1
                    });
                    Console.WriteLine(ProductName);
                    Console.WriteLine(Price);
                }

                HttpContext.Session.SetObjectAsJson("Cart", cart);

                return RedirectToAction("Cart", "Cart");
            }
        
            public IActionResult Cart()
            {
                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
                return View(cart);
            }

            public IActionResult UpdateCart(int ProductId, int Quantity)
            {
                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

                var item = cart.Find(c => c.ProductId == ProductId);
                if (item != null)
                {
                    item.Quantity = Quantity;
                }

                HttpContext.Session.SetObjectAsJson("Cart", cart);

                return RedirectToAction("Cart");
            }

            public IActionResult RemoveFromCart(int ProductId)
            {
                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

                var item = cart.Find(c => c.ProductId == ProductId);
                if (item != null)
                {
                    cart.Remove(item);
                }

                HttpContext.Session.SetObjectAsJson("Cart", cart);

                return RedirectToAction("Cart");
            }

       
    }
}
