namespace KuranGuide.Web.Models
{
    public class HadisDetailViewModel
    {
        public HadisViewModel Hadis { get; set; }
        public TemaViewModel? Tema { get; set; }
        public bool IsFavorited { get; set; }

    }
}
