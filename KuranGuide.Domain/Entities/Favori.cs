using KuranGuide.Domain.Common;

namespace KuranGuide.Domain.Entities
{
    public class Favori : BaseEntity
    {
        public int UserId { get; set; }
        public string Type { get; set; } // "ayet" veya "hadis" - FavoriType enum'a gecis icin migration gerekli
        public int RefId { get; set; }

        // Navigation
        public Kullanici Kullanici { get; set; }
    }
}
