using KuranGuide.Application.DTOs.General;
using KuranGuide.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KuranGuide.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly KuranGuideDbContext _context;

        public SearchController(KuranGuideDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
                return BadRequest("Arama için en az 2 karakter giriniz.");

            var term = q.Trim().ToLower();

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

            // 4. HADİSLER
            var hadisler = await _context.Hadisler
                .AsNoTracking()
                .Where(x => x.Metin.ToLower().Contains(term))
                .Select(x => new SearchResultItem
                {
                    Id = x.Id,
                    Title = "Hadis-i Şerif",
                    Content = x.Metin,
                    Type = "hadis",
                    Url = $"/Hadis/Detay/{x.Id}",
                    BadgeText = x.Kaynak
                })
                .Take(10)
                .ToListAsync();

            // Sonuçları paketle
            var response = new SearchResponseDto
            {
                Sureler = sureler,   // Artık .Result demeye gerek yok, direkt değişken
                Temalar = temalar,
                Ayetler = ayetler,
                Hadisler = hadisler
            };

            return Ok(response);
        }
    }
}