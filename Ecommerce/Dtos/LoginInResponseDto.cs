namespace Ecommerce.Dtos
{
    public class LoginInResponseDto
    {
        public bool Success { get; set; } = false;
        public string Token { get; set; } = null;
        public string Message { get; set; } = null;
        public UserDto User { get; set; } = null;

       

    }

    public class UserDto
    {
        public string Id { get; set; } = null;
        public string UserName { get; set; } = null;
        public string Email { get; set; } = null;
    }
}
