namespace KuranGuide.Web.Models
{
    public class HadisViewModel
    {
        public int Id { get; set; }
        public string Metin { get; set; }
        public string Kaynak { get; set; }
        public string? Aciklama { get; set; }
        public int? TemaId { get; set; }
        public string? TemaAdi { get; set; }
        public string? TemaAciklama { get; set; }
    }

}
