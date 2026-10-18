
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        // Default ASP.NET Core Identity user IDs are strings
        public string UserId { get; set; } = string.Empty;

        public int ProductId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }

        public Product? Product { get; set; }
    }
}
