using KuranGuide.Domain.Common;

namespace KuranGuide.Domain.Entities
{
    public class Tema : BaseEntity
    {
        public string TemaAdi { get; set; }
        public string Aciklama { get; set; }
        public string AnahtarKelimeler { get; set; }
        public string Icon { get; set; }

        // Navigation Properties
        public ICollection<Ayet> Ayetler { get; set; }
        public ICollection<Hadis> Hadisler { get; set; }
    }
}
