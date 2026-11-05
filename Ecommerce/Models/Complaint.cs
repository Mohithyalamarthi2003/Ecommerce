using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        
    }
}