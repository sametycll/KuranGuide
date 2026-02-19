using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Domain.Entities;
using KuranGuide.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KuranGuide.Infrastructure.Repositories
{
    public class FavoriRepository : GenericRepository<Favori>, IFavoriRepository
    {
        private readonly KuranGuideDbContext _db;

        public FavoriRepository(KuranGuideDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task<Favori?> GetByUserAndRefAsync(int userId, string type, int refId)
        {
            return await _db.Favoriler
                .FirstOrDefaultAsync(f =>
                    f.UserId == userId &&
                    f.Type == type &&
                    f.RefId == refId
                );
        }

        public async Task<List<Favori>> GetUserFavoritesAsync(int userId)
        {
            return await _db.Favoriler
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }
    }
}
