namespace Ecommerce.Services.Interface
{
    public interface ILogin
    {
        public Task<string> login(string email, string password);
    }
}
