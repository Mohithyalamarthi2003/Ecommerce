using Ecommerce.Data;
using Ecommerce.Dtos;
using Ecommerce.Services.Implementation;
using Ecommerce.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Productapi : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductView _productview;

        private readonly ICreateProduct _createproduct;
        private readonly IProductEdit _productedit;

        private readonly ISingleproduct _singleproduct;

        private readonly IProductDelete _productdelete;

        private readonly IJwtService _jwtService;

        private readonly ILogin _Login;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private object _userManger;

        public Productapi(ApplicationDbContext context, IProductView productview, ICreateProduct createproduct, IProductEdit productedit, ISingleproduct singleproduct, IProductDelete productdelete, IJwtService jwtService, ILogin login, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _productview = productview;
            _createproduct = createproduct;
            _productedit = productedit;
            _singleproduct = singleproduct;
            _productdelete = productdelete;
            _jwtService = jwtService;
            _Login = login;
            _userManager = userManager;
            _signInManager = signInManager;

        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {

            var products = await _productview.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> product(int id)
        {
            var product = await _singleproduct.product(id);
            return Ok(product);
        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> EditProduct(int id, ProductUpdateDto dto)
        {
            var product = await _productedit.EditProduct(id, dto);

            return Ok(product);
        }


        [HttpDelete("{id:int}")]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productdelete.DeleteProduct(id);

            return Ok(product);
        }

        [HttpPost]

        public async Task<IActionResult> CreateProduct([FromForm] Productcreatedto dto)
        {
            var product = await _createproduct.CreateProduct(dto);
            return Ok(product);
        }


        //Testing the login functionality with the jwt token 


        

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if(user == null)
            {
                return Unauthorized(new LoginInResponseDto
                {
                    Success = false,
                    Message = "Invalid Email or password",
                    Token = null
                });
            }
            var result = await _Login.login(dto.Email, dto.Password);

            var token =  _jwtService.GenerateToken(user);

            return Ok(new LoginInResponseDto
            {
                Success = true,
                Message = "Login Successfull",
                Token = token,
                User = new UserDto
                {
                    Id         = user.Id,
                    UserName   = user.UserName ?? user.Email,
                    Email      = user.Email ?? ""
                }
            });
        }




    }
}
