namespace KuranGuide.Web.Models
{
    public class AyetViewModel
    {
        public int Id { get; set; }
        public int SureId { get; set; }
        public string SureAdi { get; set; }
        public int AyetNo { get; set; }
        public string ArapcaMetin { get; set; }
        public string Meal { get; set; }
        public string Aciklama { get; set; }
        public string TemaAdi { get; set; }
        public int? TemaId { get; set; }
    }

} 
