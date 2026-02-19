using KuranGuide.Application.DTOs.Tema;
using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Domain.Entities;
using KuranGuide.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KuranGuide.Infrastructure.Repositories
{
    public class TemaRepository : GenericRepository<Tema>, ITemaRepository 
    {
        public TemaRepository(KuranGuideDbContext context) : base(context)
        {
        }
        public async Task<List<TemaDto>> GetAllWithCountsAsync()
        {    
            return await _context.Temalar
                .Select(t => new TemaDto
                {
                    Id = t.Id,
                    TemaAdi = t.TemaAdi,
                    Aciklama = t.Aciklama,
                    Icon = t.Icon,
                    AnahtarKelimeler = t.AnahtarKelimeler,

                    // EF Core burayı SQL COUNT sorgusuna çevirir
                    AyetSayisi = t.Ayetler.Count(),
                    HadisSayisi = t.Hadisler.Count()
                })
                .ToListAsync();
        }

    }
}
