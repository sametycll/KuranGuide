namespace KuranGuide.Application.DTOs.Favori
{
    public class AddFavoriDto
    {
        public string Type { get; set; }  // "ayet" veya "hadis"
        public int RefId { get; set; }    // AyetID veya HadisID
    }
}
