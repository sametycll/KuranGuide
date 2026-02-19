using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Domain.Entities;
using KuranGuide.Infrastructure.Context;
using KuranGuide.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class AyetRepository : GenericRepository<Ayet>, IAyetRepository
{
    public AyetRepository(KuranGuideDbContext context) : base(context)
    {
    }
    public async Task<List<Ayet>> GetAllWithTemaAsync()
    {
        return await _context.Ayetler
            .Include(a => a.Tema) // SQL JOIN işlemi yapar
            .OrderBy(a => a.SureId)
            .ThenBy(a => a.AyetNo)
            .ToListAsync();
    }

    public async Task<Ayet> GetGununAyetiAsync()
    {
        // 1. Toplam sayıyı al (Hızlı Count sorgusu)
        var totalCount = await _context.Ayetler.CountAsync();
        if (totalCount == 0) return null;

        // 2. Günün indexini hesapla
        int index = DateTime.UtcNow.DayOfYear % totalCount;

        // 3. Sadece O satırı getir (Sure bilgisiyle beraber JOIN yapıp getirir)
        return await _context.Ayetler
            .Include(a => a.Sure) // Sure bilgisini de çek (Ekstra sorguya gerek kalmaz)
            .OrderBy(a => a.Id)   // Skip kullanmak için sıralama şarttır
            .Skip(index)          // Index kadar atla
            .Take(1)              // 1 tane al
            .FirstOrDefaultAsync();
    }

}
