using LiquorStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using  LiquorStore.Extensions;

namespace LiquorStore.Controllers
{
    public class CartController : Controller
    {
        private readonly LSContext _context;

        public CartController(LSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null || product.StockQuantity < 1)
            {
                return Json(new { success = false, message = "Product unavailable" });
            }

            var cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                });
            }

            HttpContext.Session.SetObject("Cart", cart);
            return Json(new { 
                success = true, 
                itemCount = cart.Items.Sum(i => i.Quantity),
                grandTotal = cart.GrandTotal
            });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObject<Cart>("Cart");
            if (cart == null) return Json(new { success = false });

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Items.Remove(item);
                HttpContext.Session.SetObject("Cart", cart);
            }

            return Json(new { 
                success = true, 
                itemCount = cart.Items.Sum(i => i.Quantity),
                grandTotal = cart.GrandTotal
            });
        }
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObject<Cart>("Cart");
            if (cart == null || !cart.Items.Any()) return RedirectToAction("Index");

            // Calculate totals
            var subtotal = cart.GrandTotal;
            var tax = subtotal * 0.13m;
            var shipping = 4.99m;
            var total = subtotal + tax + shipping;

            // Store in TempData to pass to the next action
            TempData["Total"] = total;

            return View();
        }


    }
}