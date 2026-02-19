namespace KuranGuide.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Ad { get; set; } 
        public string Soyad { get; set; } 
        public int UserId { get; set; }
    }
}