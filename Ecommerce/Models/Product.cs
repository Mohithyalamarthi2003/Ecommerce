using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ecommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";

        [Required]
        [MinLength(20)]
        public string Description { get; set; } = "";

        [Required]
        public int AvailableQuantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = "";

        [NotMapped]
        public IFormFile ? Image { get; set; } 

    }
}
