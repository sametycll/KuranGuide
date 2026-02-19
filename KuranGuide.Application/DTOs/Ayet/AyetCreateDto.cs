namespace KuranGuide.Application.DTOs.Ayet
{
    public class AyetCreateDto
    {
        public int SureId { get; set; }
        public int AyetNo { get; set; }
        public string ArapcaMetin { get; set; }
        public string Meal { get; set; }
        public string Aciklama { get; set; }
        public int? TemaId { get; set; }
    }
}
