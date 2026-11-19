using Ecommerce.Dtos;
namespace Ecommerce.Services.Interface
{
    public interface IProductEdit
    {
        public Task<ProductUpdateDto> EditProduct(int id, ProductUpdateDto dto);
    }
}
