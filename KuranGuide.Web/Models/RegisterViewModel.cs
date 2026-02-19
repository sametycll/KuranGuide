using System.ComponentModel.DataAnnotations;

namespace KuranGuide.Web.Models
{
    public class RegisterViewModel
    {
        // YENİ: Ad Alanı
        [Required(ErrorMessage = "Ad alanı gereklidir.")]
        public string Ad { get; set; }

        // YENİ: Soyad Alanı
        [Required(ErrorMessage = "Soyad alanı gereklidir.")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "E-posta gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        // Backend'deki Regex ile aynı kuralı buraya da koyuyoruz ki gereksiz istek gitmesin
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
             ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı gereklidir.")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}