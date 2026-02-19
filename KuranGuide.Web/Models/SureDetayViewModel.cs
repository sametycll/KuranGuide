namespace KuranGuide.Web.Models
{
    public class SureDetayViewModel
    {
        public int Id { get; set; }
        public int SureNo { get; set; }
        public string SureAdi { get; set; }
        public string ArapcaAdi { get; set; }
        public string Yer { get; set; }
        public List<AyetViewModel> Ayetler { get; set; }
    }
}
