namespace KuranGuide.Application.DTOs.Hadis
{
    public class HadisCreateDto
    {
        public string Kaynak { get; set; }
        public string Metin { get; set; }
        public string Aciklama { get; set; }
        public int TemaId { get; set; }
    }
}
