
using Ecommerce.Data;
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
            // 1. Find the product using its primary key
            var item = await _context.Products.FindAsync(id);

            if (item == null)
            {
                return NotFound("Product not found.");
            }

            // 2. Get the currently logged-in user's ID
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // 3. Check whether this product is already in this user's cart
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c =>
                    c.UserId == userId &&
                    c.ProductId == id);

            // 4. Increase the quantity if the item already exists
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                // 5. Otherwise, create a new cart item
                cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = item.Id,
                    Quantity = 1
                };

                _context.CartItems.Add(cartItem);
            }

            // 6. Save changes to the database
            await _context.SaveChangesAsync();

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
            cartitem.Quantity = model.Quantity;

            await _context.SaveChangesAsync();

            return RedirectToAction("ViewCart","AddToCart");
        }

    }
}
