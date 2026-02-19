using KuranGuide.Domain.Common;

namespace KuranGuide.Domain.Entities
{
    public class Ayet : BaseEntity
    {
      
        public int AyetNo { get; set; }
        public string ArapcaMetin { get; set; }
        public string Meal { get; set; }
        public string? Aciklama { get; set; }

       
        // FOREIGN KEY: SURE (Zorunlu)
        public int SureId { get; set; }
        public Sure Sure { get; set; }

        // FOREIGN KEY: TEMA (Opsiyonel - Nullable)
        // Çünkü her ayet bizim belirlediğimiz bir duygu temasında olmayabilir.
        // Amaç Kuran okumaksa, tema boş olabilir.
        public int? TemaId { get; set; }
        public Tema? Tema { get; set; }
    }
}
