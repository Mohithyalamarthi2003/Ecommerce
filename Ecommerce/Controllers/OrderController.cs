using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = _context.CartItems
                .Where(c => c.UserId == userId)
                .ToList();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Cart is empty";
                return RedirectToAction("ViewCart", "AddToCart");
            }

            foreach (var cartItem in cartItems)
            {
                var product = await _context.Products
                    .FindAsync(cartItem.ProductId);

                if (product == null)
                {
                    continue;
                }

                // Validation
                if (cartItem.Quantity > product.AvailableQuantity)
                {
                    TempData["Error"] =
                        $"Only {product.AvailableQuantity} items available for {product.Name}";

                    return RedirectToAction("ViewCart", "AddToCart");
                }

                // Reduce stock
                product.AvailableQuantity -= cartItem.Quantity;
            }

            // Remove cart items after successful checkout
            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Order placed successfully";

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]

        public async Task <IActionResult> CheckOut()
        {
            return View();
        }
    }
}
