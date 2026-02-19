namespace KuranGuide.Web.Models
{
    public class TemaDetailViewModel
    {
        public TemaViewModel Tema { get; set; }

        public List<AyetViewModel> Ayetler { get; set; } = new();
        public List<HadisViewModel> Hadisler { get; set; } = new();
    }
}
