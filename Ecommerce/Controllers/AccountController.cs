using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using Ecommerce.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;



namespace Ecommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        //http get request for the register page 

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

       //http post request for the Register Page 
        
        [HttpPost]

        public async Task <IActionResult> Register(RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            //checking the user is existing or new user 

            var existing = await _userManager.FindByEmailAsync(model.Email);

            if (existing! == null)
            {
                ModelState.AddModelError("", "Email Id already exists please try login");
                return View(model);
            }

            var User = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                
            };


            var result = await _userManager.CreateAsync(User, model.Password);

            if(result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        //http get request for the login page

        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }

        //http post request for the login page 

        [HttpPost]

        public async Task <IActionResult> Login(LoginModel model)
        {

            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync
                 (
                 model.Email,
                 model.Password,
                 false,
                 false
            );
            if(result.Succeeded)
            {
                return RedirectToAction("Privacy", "Home");
            }
            ModelState.AddModelError("", "Please Enter valid credentials");
            return View();
        }
    }
}
