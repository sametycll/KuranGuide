namespace KuranGuide.Web.Models
{
    public class LoginResponseViewModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
    }
}
