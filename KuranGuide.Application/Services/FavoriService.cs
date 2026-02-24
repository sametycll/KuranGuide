using KuranGuide.Application.DTOs.Favori;
using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Services
{
    public class FavoriService : IFavoriService
    {
        private readonly IFavoriRepository _repo;

        public FavoriService(IFavoriRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Favori>> GetUserFavoritesAsync(int userId)
        {
            return await _repo.FindAsync(f => f.UserId == userId);
        }

        public async Task<Favori> AddFavoriteAsync(Favori favori)
        {
            var created = await _repo.AddAsync(favori);
            await _repo.SaveChangesAsync();
            return created;
        }


        public async Task<bool> RemoveFavoriteAsync(int favId, int userId)
        {
            var record = await _repo.GetByIdAsync(favId);
            if (record == null || record.UserId != userId)
                return false;

            await _repo.DeleteAsync(record);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<ToggleFavoriResult> ToggleFavoriteAsync(int userId, string type, int refId)
        {
            var existing = await _repo.GetByUserAndRefAsync(userId, type, refId);

            if (existing != null)
            {
                await _repo.DeleteAsync(existing);
                await _repo.SaveChangesAsync();
                return new ToggleFavoriResult
                {
                    Added = false,
                    FavoriId = existing.Id
                };
            }

            var yeni = new Favori
            {
                UserId = userId,
                Type = type,
                RefId = refId,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repo.AddAsync(yeni);
            await _repo.SaveChangesAsync();

            return new ToggleFavoriResult
            {
                Added = true,
                FavoriId = created.Id
            };
        }






    }
}
