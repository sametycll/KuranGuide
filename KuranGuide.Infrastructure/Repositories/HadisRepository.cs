using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Domain.Entities;
using KuranGuide.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KuranGuide.Infrastructure.Repositories
{
    public class HadisRepository : GenericRepository<Hadis>, IHadisRepository
    {
        public HadisRepository(KuranGuideDbContext context) : base(context)
        {
        }

        public async Task<List<Hadis>> GetAllWithTemaAsync()
        {
            return await _context.Hadisler
                .Include(a => a.Tema) // SQL JOIN işlemi yapar
                .OrderBy(a => a.Id)
                .ToListAsync();
        }
    }
}
