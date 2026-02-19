using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Domain.Entities;
using KuranGuide.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KuranGuide.Infrastructure.Repositories
{
    public class SureRepository : GenericRepository<Sure>, ISureRepository
    {
        public SureRepository(KuranGuideDbContext context) : base(context)
        {
        }

        // Sure'yi ayetleriyle beraber getiren özel metot
        public async Task<Sure> GetBySureNoWithAyetlerAsync(int sureNo)
        {
            return await _context.Sureler
                .Include(s => s.Ayetler) // Ayetleri de çek
                .FirstOrDefaultAsync(s => s.SureNo == sureNo);
        }
    }
}