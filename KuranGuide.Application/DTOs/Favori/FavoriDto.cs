namespace KuranGuide.Application.DTOs.Favori
{
    public class FavoriDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int RefId { get; set; }
        public string TextPreview { get; set; }
        public string SourceInfo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
