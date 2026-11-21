using Ecommerce.Data;

namespace Ecommerce.Services.Interface
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
    }
}
