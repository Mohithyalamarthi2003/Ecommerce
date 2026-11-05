using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;


namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController (ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var product = _context.Products.ToList();
            return View(product);
        }

        public IActionResult Contact ()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Complaint()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var complaints = await _context.Complaints
       .Where(c => c.UserId == userId)
       .OrderByDescending(c => c.CreatedAt)
       .ToListAsync();

            if (complaints != null)
            {
                return View(complaints);
            }

            return RedirectToAction("NewComplaint", "Home");
            
               
            
            
        }

        [Authorize]
        

        public IActionResult NewComplaint()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complaint(Complaint model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // These values must come from the logged-in user/server,
            // not from values posted by the browser.
            model.UserId = userId;
            model.CreatedAt = DateTime.UtcNow;

            

            if (!ModelState.IsValid)
            {
                var complaints = await _context.Complaints
                    .Where(c => c.UserId == userId)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                return View(complaints);
            }

            _context.Complaints.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Complaint));
        }

        //giving the editing feature for the complaints module 

       

       
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditComplaint(int id)
        {
            var currentUserId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var complaint =
                await _context.Complaints
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.UserId == currentUserId);

            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        [HttpPost]
        public async Task<IActionResult> EditComplaint(Complaint model)
        {
            var complaint = await _context.Complaints
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (complaint == null)
            {
                return NotFound();
            }

            complaint.Email = model.Email;
            complaint.Subject = model.Subject;
            complaint.Description = model.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction("Complaint");
        }



        //controller for deleting the complaint 

        [HttpGet]
        public async Task <IActionResult> DeleteComplaint(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var complaint = await _context.Complaints.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
                if(complaint==null)
            {
                return NotFound();
            }
                return View(complaint);
        }

        [HttpPost]

        public async Task <IActionResult> DeleteComplaint(Complaint model)
        {
            var complaint =  _context.Complaints.FirstOrDefault(x => x.Id == model.Id);
            _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync();
            return RedirectToAction("Complaint");
        }


        //the field which is returning the guide view with the html code 

        public IActionResult Guide()
        {
            return View();
        }
    }
}
