using KuranGuide.Application.DTOs.Ayet;
using KuranGuide.Application.DTOs.Hadis;

namespace KuranGuide.Application.DTOs.Tema
{
    public class TemaWithContentDto
    {
        public int Id { get; set; }
        public string TemaAdi { get; set; }
        public string Aciklama { get; set; }
        public string AnahtarKelimeler { get; set; }
        public string Icon { get; set; }


        public List<AyetDto> Ayetler { get; set; }
        public List<HadisDto> Hadisler { get; set; }
    }
}
