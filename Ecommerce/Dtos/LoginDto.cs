using System.ComponentModel.DataAnnotations;
namespace Ecommerce.Dtos
{
    public class LoginDto
    {
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null;

        [Required]
        [MinLength(6, ErrorMessage = "The password must be at least 6 characters")] 
        public string Password { get; set; } = null;

    }
}
