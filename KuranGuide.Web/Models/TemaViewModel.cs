namespace KuranGuide.Web.Models
{
    public class TemaViewModel
    {
        public int Id { get; set; }
        public string TemaAdi { get; set; }
        public string Aciklama { get; set; }
        public string AnahtarKelimeler { get; set; }
        public string Icon { get; set; }


        // Dinamik eklenen alanlar
        public int AyetSayisi { get; set; }
        public int HadisSayisi { get; set; }
    }


}
