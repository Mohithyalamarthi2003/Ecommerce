using Ecommerce.Dtos;
namespace Ecommerce.Services.Interface
{
    public interface ISingleproduct
    {
        public Task<ProductDto> product(int id);
    }
}
