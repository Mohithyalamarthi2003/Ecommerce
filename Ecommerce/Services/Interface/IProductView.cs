using Ecommerce.Models;
using Ecommerce.Data;
using Ecommerce.Dtos;
namespace Ecommerce.Services.Interface
{
    public interface IProductView
    {
        public Task<List<ProductDto>> GetProducts();
    }
}
