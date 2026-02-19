using KuranGuide.Application.DTOs.Ayet;

namespace KuranGuide.Application.DTOs.Sure
{
    public class SureDetailDto
    {
        public int Id { get; set; }
        public int SureNo { get; set; }
        public string SureAdi { get; set; }
        public string ArapcaAdi { get; set; }
        public string Yer { get; set; }

        // Bu sureye ait ayetler listesi
        public List<AyetDto> Ayetler { get; set; }
    }
}