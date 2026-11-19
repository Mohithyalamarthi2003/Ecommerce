using Ecommerce.Data;
using Ecommerce.Dtos;
using Ecommerce.Services.Interface;

namespace Ecommerce.Services.Implementation
{
    public class ProductEdit : IProductEdit
    {
        private readonly ApplicationDbContext _context;

        public ProductEdit(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductUpdateDto?> EditProduct(int id, ProductUpdateDto dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return null;
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.AvailableQuantity = dto.AvailableQuantity;

            await _context.SaveChangesAsync();

            return new ProductUpdateDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                AvailableQuantity = product.AvailableQuantity
            };
        }
    }
}