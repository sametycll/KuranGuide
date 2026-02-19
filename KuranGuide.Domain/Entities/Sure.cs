using KuranGuide.Domain.Common;

namespace KuranGuide.Domain.Entities
{
    public class Sure : BaseEntity
    {
        // SureNo'yu ID olarak kullanmak yerine ayrı tutmak daha esnek olabilir ama
        // Genelde SureNo (1-114) sabittir. Yine de BaseEntity'den gelen Id'yi Primary Key,
        // SureNo'yu ise verisi olarak tutalım.
        public int SureNo { get; set; }

        public string SureAdi { get; set; }      // Örn: Bakara
        public string ArapcaAdi { get; set; }    // Örn: البقرة
        public int InisSirasi { get; set; }      // Örn: 87
        public int AyetSayisi { get; set; }      // Örn: 286
        public int CuzNo { get; set; }           // Başlangıç Cüzü
        public int SayfaNo { get; set; }         // Başlangıç Sayfası
        public string Yer { get; set; }          // Mekke / Medine

        // Navigation Property (Bire-Çok İlişki)
        // Bir surenin ÇOK ayeti olur.
        public ICollection<Ayet> Ayetler { get; set; }
    }
}