namespace GastappApi.DTOs
{
    public class UserLoginInfo
    {
        public string Token { get; set; } = null!;
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
