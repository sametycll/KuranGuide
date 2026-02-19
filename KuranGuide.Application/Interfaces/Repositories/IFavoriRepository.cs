using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Interfaces.Repositories
{
    public interface IFavoriRepository : IGenericRepository<Favori> {
        Task<Favori?> GetByUserAndRefAsync(int userId, string type, int refId);
        Task<List<Favori>> GetUserFavoritesAsync(int userId);
    }
}
