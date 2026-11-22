using Ecommerce.Data;
using Ecommerce.Dtos;

namespace Ecommerce.Services.Interface
{
    public interface IJwtService   
    {
        string GenerateToken(ApplicationUser user);
    }
}
