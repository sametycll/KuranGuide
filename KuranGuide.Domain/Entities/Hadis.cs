using KuranGuide.Domain.Common;

namespace KuranGuide.Domain.Entities
{
    public class Hadis : BaseEntity
    {
        public string Kaynak { get; set; }
        public string Metin { get; set; }
        public string? Aciklama { get; set; }

        // FK
        public int? TemaId { get; set; }

        // Navigation
        public Tema? Tema { get; set; }
    }
}
