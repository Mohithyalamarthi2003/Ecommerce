using Ecommerce.Data;
using Ecommerce.Dtos;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Services.Interface;
namespace Ecommerce.Services.Implementation
{
    public class createProduct : ICreateProduct
    {
        private readonly ApplicationDbContext _context;

        public createProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProduct(Productcreatedto dto)
        {
            var filename = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);

            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", filename);

            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            var imageurl = $"images/{filename}";

                var product = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.price,
                    AvailableQuantity = dto.AvailableQuantity,
                    ImageUrl = imageurl
                };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
           
        }
    }
}
