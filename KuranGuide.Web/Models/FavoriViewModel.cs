namespace KuranGuide.Web.Models
{
    public class FavoriViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; } // ayet/hadis
        public int RefId { get; set; }
        public string TextPreview { get; set; }
        public string SourceInfo { get; set; }
    }

    public class FavoriPageViewModel
    {
        public List<FavoriViewModel> Ayetler { get; set; }
        public List<FavoriViewModel> Hadisler { get; set; }
    }

}
