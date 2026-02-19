namespace KuranGuide.Application.DTOs.General
{
    public class SearchResponseDto
    {
        public List<SearchResultItem> Sureler { get; set; } = new();
        public List<SearchResultItem> Temalar { get; set; } = new();
        public List<SearchResultItem> Ayetler { get; set; } = new();
        public List<SearchResultItem> Hadisler { get; set; } = new();

        // Sonuç var mı kontrolü için yardımcı özellik
        public bool HasResults => Sureler.Any() || Temalar.Any() || Ayetler.Any() || Hadisler.Any();
    }

    public class SearchResultItem
    {
        public int Id { get; set; }
        public string Title { get; set; }     // Sure Adı, Tema Adı vb.
        public string Content { get; set; }   // Ayet Meali, Hadis Metni vb.
        public string Type { get; set; }      // "sure", "tema", "ayet", "hadis"
        public string Url { get; set; }       // Tıklayınca gideceği yer

        // Ekstra Bilgiler
        public string BadgeText { get; set; } // "Bakara 255", "Buhari" vb.
    }
}