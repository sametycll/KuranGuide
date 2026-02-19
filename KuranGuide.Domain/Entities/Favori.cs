using KuranGuide.Domain.Common;

namespace KuranGuide.Domain.Entities
{
    public class Favori : BaseEntity
    {
        public int UserId { get; set; }
        public string Type { get; set; } // "ayet" veya "hadis"
        public int RefId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Kullanici Kullanici { get; set; }
    }
}
