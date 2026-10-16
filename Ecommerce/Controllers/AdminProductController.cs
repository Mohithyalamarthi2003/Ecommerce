using Ecommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Ecommerce.Models;
using Microsoft.Identity.Client;

namespace Ecommerce.Controllers
{
    public class AdminProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task <IActionResult> Create(Product product)
        {
            if(!ModelState.IsValid)
            {
                return View(product);
            }

            var productitem = await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");



        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
