using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Services.Interface;
using Ecommerce.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ecommerce.Services.Implementation
{
    public class ProductView : IProductView
    {
        private readonly ApplicationDbContext _context;
        public ProductView(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductDto>> GetProducts()
        {
            var product = await _context.Products.Select(p=> new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ImageUrl = p.ImageUrl
            }).ToListAsync();
            return product;
        }
    }
}
