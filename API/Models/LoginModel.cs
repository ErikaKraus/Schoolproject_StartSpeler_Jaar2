namespace API.Models
{
    public class LoginModel
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public bool RememberMe { get; set; }
    }
}
