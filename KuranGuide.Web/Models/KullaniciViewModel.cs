namespace KuranGuide.Web.Models
{
    public class KullaniciViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
