using KuranGuide.Domain.Common;

namespace KuranGuide.Domain.Entities
{
    public class Kullanici : BaseEntity
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "User";

        public ICollection<Favori> Favoriler { get; set; }
    }
}
