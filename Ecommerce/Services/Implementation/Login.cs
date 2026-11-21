using Ecommerce.Services.Implementation;
using Ecommerce.Data;
using Ecommerce.Services.Interface;
using Microsoft.AspNetCore.Identity;
namespace Ecommerce.Services.Implementation
{
    public class Login : ILogin
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;

        public Login(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<string> login(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password,false,false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);
                var token = _jwtService.GenerateToken(user);
                return token;

            }
            else
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }


        }
    }
}