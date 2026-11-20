using Ecommerce.Data;
using Ecommerce.Dtos;
using Ecommerce.Services.Interface;
namespace Ecommerce.Services.Implementation
{
    public class ProductDelete : IProductDelete
    {
        private readonly ApplicationDbContext _context;

        public ProductDelete(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteDto> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if(product == null)
            {
                return null;
            }
            var result = new DeleteDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description
            };

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return result;
        }
    }
}
