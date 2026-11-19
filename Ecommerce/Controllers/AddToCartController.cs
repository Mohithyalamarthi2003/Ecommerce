
using Ecommerce.Data;
using Ecommerce.Migrations;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    
    public class AddToCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddToCartController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cart(int id)
        {
            var item = await _context.Products.FindAsync(id);

            if (item == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("Index", "Home");
            }

            if (item.AvailableQuantity == 0)
            {
                TempData["Error"] = "Product is out of stock!";
                return RedirectToAction("Index", "User");
            }

            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c =>
                    c.UserId == userId &&
                    c.ProductId == id);

            if (cartItem != null)
            {
                if (cartItem.Quantity >= item.AvailableQuantity)
                {
                    TempData["Error"] = "Cannot add more items. Stock limit reached.";
                    return RedirectToAction("ViewCart", "AddToCart");
                }

                cartItem.Quantity++;
            }
            else
            {
                cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = item.Id,
                    AvailableQuantity = item.AvailableQuantity,
                    Quantity = 1
                };

                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Product added to cart.";

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> ViewCart()
        {
            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return View(cartItems);
        }

        [Authorize]
        [HttpPost]

        public async Task <IActionResult> Remove(int id)
        {

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartitem = _context.CartItems.FirstOrDefault(c => c.Id == id && c.UserId == userId);

            if (cartitem==null)
            {
                return NotFound();
            }
            _context.CartItems.Remove(cartitem);
             await  _context.SaveChangesAsync();
             return RedirectToAction("ViewCart","AddToCart");
        }

        [Authorize]
        [HttpPost]
        public async Task <IActionResult> Update(CartView model,int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId==null)
            {
                return NotFound();
            }


            if (!ModelState.IsValid || model.Quantity < 1)
            {
                return BadRequest("Enter a valid quantity greater than zero.");
            }


            var cartitem = _context.CartItems.FirstOrDefault(c => c.Id == id && c.UserId == userId);
            if (cartitem == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(cartitem.ProductId);
           
            if (model.Quantity > product.AvailableQuantity)
            {
                return BadRequest("Quantity exceeds available stock.");
            }

            cartitem.Quantity = model.Quantity;

            await _context.SaveChangesAsync();

            return RedirectToAction("ViewCart","AddToCart");
        }

    }
}
