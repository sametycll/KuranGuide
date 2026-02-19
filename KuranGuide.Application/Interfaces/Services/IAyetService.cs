using KuranGuide.Application.DTOs.Ayet;
using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Interfaces.Services
{
    public interface IAyetService
    {
        Task<IEnumerable<Ayet>> GetAllAsync();
        Task<Ayet> GetByIdAsync(int id);
        Task<Ayet> CreateAsync(Ayet entity);
        Task<Ayet> UpdateAsync(Ayet entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Ayet>> GetByTemaIdAsync(int temaId);
        Task<IEnumerable<Ayet>> SearchAsync(string text);
        Task<IEnumerable<Ayet>> GetAllWithTemaAsync();
        Task<AyetDto> GetGununAyetiAsync();
    }
}
