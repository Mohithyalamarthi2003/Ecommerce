namespace Ecommerce.Models
{
    public class CartView
    {
       public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal Unitprice { get; set; }

        public int Quantity { get; set; }

        public decimal total => Unitprice * Quantity;

       
    }
}
