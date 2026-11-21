//using Ecommerce.Data;
//using Ecommerce.Dtos;
//using Ecommerce.Services;
//using Ecommerce.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.Data;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace Ecommerce.Controllers
//{
   
//    [ApiController]
//    [Route("api/[controller]")]
//    public class DisplayController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;

//        //private readonly Jwtservice _jwtService;


//        public DisplayController(
//            ApplicationDbContext context,
//            UserManager<ApplicationUser> userManager,
//            SignInManager<ApplicationUser> signInManager,
//            //Jwtservice jwtService)
//        {
//            _context = context;
//            _userManager = userManager;
//            _signInManager = signInManager;
//            //_jwtService = jwtService;   
//        }

//        [Authorize]
//        [HttpGet]
//        public async Task <IActionResult> Products()
//        {
//            var product = await _context.Products.Select(p => new ProductDto
//            {
//                Id = p.Id,
//                Name = p.Name,
//                Description = p.Description
//            }).ToListAsync();

//            return Ok(product);
            
//        }


//        [HttpGet("{id:int}")]
//        public async Task <IActionResult> Product(int id)
//        {
//            var product =  _context.Products.Where(p => p.Id == id).Select(p => new ProductDto
//            {
//                Id = p.Id,
//                Name = p.Name,
//                Description = p.Description
//            }).FirstOrDefault();

//            return Ok(product);
//        }


//        [HttpPut("{id:int}")]

//        public async Task <IActionResult> EditProduct(int id, ProductUpdateDto dto)
//        {
//            var product = await _context.Products.FindAsync(id);

//            product.Name = dto.Name;
//            product.Description = dto.Description;
//            product.Price = dto.Price;
//            product.AvailableQuantity = dto.AvailableQuantity;

//            return Ok(product);
//        }

//        [HttpPost("create")]
//        public async Task<IActionResult> Create([FromBody] Productcreatedto dto)
//        {
//            if (dto == null)
//            {
//                return BadRequest("Invalid product data.");
//            }

//            var product = new Product
//            {
//                Name = dto.Name,
//                Description = dto.Description,
//                AvailableQuantity = dto.AvailableQuantity,
//                Price = dto.price
//            };

//            _context.Products.Add(product);
//            await _context.SaveChangesAsync();

//            return Ok(product);
//        }

//        //next we are going to implement the add to login 
//        [AllowAnonymous]
//        [HttpPost("login")]
//        public async Task<IActionResult> Login(
//       LoginModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var user = await _userManager
//                .FindByEmailAsync(model.Email);

//            if (user == null)
//            {
//                return Unauthorized(
//                    new { message = "Invalid email or password" }
//                );
//            }

//            var passwordValid = await _userManager
//                .CheckPasswordAsync(user, model.Password);

//            if (!passwordValid)
//            {
//                return Unauthorized(
//                    new { message = "Invalid email or password" }
//                );
//            }

//            var roles = await _userManager
//                .GetRolesAsync(user);

//            var role = roles.FirstOrDefault() ?? "User";

//            //var token = _jwtService.GenerateToken(
//            //    user.Id,
//            //    user.Email!,
//            //    role
//            //);

//            return Ok(new
//            {
//                token = token,
//                userId = user.Id,
//                email = user.Email,
//                role = role
//            });
//        }
//    }
//}