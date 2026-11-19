using Ecommerce.Data;
using Ecommerce.Dtos;
using Ecommerce.Services.Interface;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ecommerce.Services.Implementation
{
    public class Singleproduct : ISingleproduct
    {
        private readonly ApplicationDbContext _context;
    public Singleproduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task <ProductDto> product(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return null;

            }

            var Product = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl
            };
            return Product;
        }

  
    }
}
