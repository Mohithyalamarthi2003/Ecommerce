namespace Ecommerce.Dtos
{
    public class Productcreatedto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int AvailableQuantity { get; set; }

        public int price { get; set; }

        public IFormFile ? Image { get; set; } 

    }
}
