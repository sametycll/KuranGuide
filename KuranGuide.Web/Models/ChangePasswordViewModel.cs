using System.ComponentModel.DataAnnotations;

namespace KuranGuide.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Yeni şifre gereklidir.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalı.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
             ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifreyi tekrar giriniz.")]
        [Compare("NewPassword", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmNewPassword { get; set; }
    }
}