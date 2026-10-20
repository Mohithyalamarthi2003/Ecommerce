using Ecommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Ecommerce.Models;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
    public class AdminProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminProductController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (product.Image != null && product.Image.Length > 0)
            {
                // Folder inside wwwroot
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");

                // Create folder if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Unique file name to avoid collisions
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(product.Image.FileName);

                // Full physical path
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Copy file to wwwroot
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.Image.CopyToAsync(stream);
                }

                // Store relative URL in DB (this is what the browser will use)
                product.ImageUrl = "/images/products/" + uniqueFileName;
            }

            var productitem = await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");



        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var product = await _context.Products.ToListAsync();
            return View(product);
        }

        //for editing of the product 

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? Image)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.AvailableQuantity = product.AvailableQuantity;
            existingProduct.Price = product.Price;

            if (Image != null && Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString()
                                     + Path.GetExtension(Image.FileName);

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                existingProduct.ImageUrl = "/images/products/" + uniqueFileName;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        //writing the code for deletion of the product 
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);

        }

        [HttpPost]

        public async Task <IActionResult> Delete(Product product)
        {
            if(!ModelState.IsValid)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]

        public async Task <IActionResult> Complaint()
        {
            var complaints = await _context.Complaints.ToListAsync();
            return View(complaints);
        }
    }
    }

