using Ecommerce.Dtos;
using Ecommerce.Models;
namespace Ecommerce.Services.Interface
{
    public interface ICreateProduct
    {
        public Task<Product> CreateProduct(Productcreatedto dto);
    }
}
