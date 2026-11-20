using Ecommerce.Dtos;
namespace Ecommerce.Services.Interface
{
    public interface IProductDelete
    {
        public Task<DeleteDto> DeleteProduct(int id);
    }
}
