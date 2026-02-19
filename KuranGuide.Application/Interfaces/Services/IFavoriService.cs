using KuranGuide.Application.DTOs.Favori;
using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Interfaces.Services
{
    public interface IFavoriService
    {
        Task<IEnumerable<Favori>> GetUserFavoritesAsync(int userId);
        Task<Favori> AddFavoriteAsync(Favori favori);
        Task<bool> RemoveFavoriteAsync(int favId, int userId);

        Task<ToggleFavoriResult> ToggleFavoriteAsync(int userId, string type, int refId);

    }
}
