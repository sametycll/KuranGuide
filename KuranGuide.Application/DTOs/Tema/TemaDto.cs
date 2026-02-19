namespace KuranGuide.Application.DTOs.Tema
{
    public class TemaDto
    {
        public int Id { get; set; }
        public string TemaAdi { get; set; }
        public string Aciklama { get; set; }
        public string Icon { get; set; }
        public string AnahtarKelimeler { get; set; }

        // Bu alanlar dolu olmalı
        public int AyetSayisi { get; set; }
        public int HadisSayisi { get; set; }
    }
}
