using KuranGuide.Application.DTOs.General;
using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KuranGuide.Infrastructure.Repositories
{
    public class SearchService : ISearchService
    {
        private readonly KuranGuideDbContext _context;

        public SearchService(KuranGuideDbContext context)
        {
            _context = context;
        }

        public async Task<SearchResponseDto> SearchAsync(string query)
        {
            var term = query.Trim().ToLower();

            // 1. SURELER
            var sureler = await _context.Sureler
                .AsNoTracking()
                .Where(x => x.SureAdi.ToLower().Contains(term) || x.ArapcaAdi.ToLower().Contains(term))
                .Select(x => new SearchResultItem
                {
                    Id = x.Id,
                    Title = x.SureAdi,
                    Content = $"{x.ArapcaAdi} - {x.Yer}",
                    Type = "sure",
                    Url = $"/Sure/Detay/{x.SureNo}",
                    BadgeText = $"{x.AyetSayisi} Ayet"
                })
                .Take(5)
                .ToListAsync();

            // 2. TEMALAR
            var temalar = await _context.Temalar
                .AsNoTracking()
                .Where(x => x.TemaAdi.ToLower().Contains(term) || x.AnahtarKelimeler.ToLower().Contains(term))
                .Select(x => new SearchResultItem
                {
                    Id = x.Id,
                    Title = x.TemaAdi,
                    Content = x.Aciklama,
                    Type = "tema",
                    Url = $"/Temalar/Detay/{x.Id}",
                    BadgeText = "Konu"
                })
                .Take(5)
                .ToListAsync();

            // 3. AYETLER
            var ayetler = await _context.Ayetler
                .AsNoTracking()
                .Include(x => x.Sure)
                .Where(x => x.Meal.ToLower().Contains(term))
                .Select(x => new SearchResultItem
                {
                    Id = x.Id,
                    Title = $"{x.Sure.SureAdi} Suresi, {x.AyetNo}. Ayet",
                    Content = x.Meal,
                    Type = "ayet",
                    Url = $"/Ayet/Detay/{x.Id}",
                    BadgeText = "Ayet"
                })
                .Take(20)
                .ToListAsync();

            // 4. HADISLER
            var hadisler = await _context.Hadisler
                .AsNoTracking()
                .Where(x => x.Metin.ToLower().Contains(term))
                .Select(x => new SearchResultItem
                {
                    Id = x.Id,
                    Title = "Hadis-i Serif",
                    Content = x.Metin,
                    Type = "hadis",
                    Url = $"/Hadis/Detay/{x.Id}",
                    BadgeText = x.Kaynak
                })
                .Take(10)
                .ToListAsync();

            return new SearchResponseDto
            {
                Sureler = sureler,
                Temalar = temalar,
                Ayetler = ayetler,
                Hadisler = hadisler
            };
        }
    }
}
