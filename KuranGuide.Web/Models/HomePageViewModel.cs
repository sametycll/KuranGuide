using KuranGuide.Web.Models;

public class HomePageViewModel
{
    public AyetViewModel GununAyeti { get; set; }
    public HadisViewModel GununHadisi { get; set; }
    public List<TemaViewModel> PopulerTemalar { get; set; }
    public bool IsFavoritedHadis { get; set; }
    public bool IsFavoritedAyet { get; set; }

}
