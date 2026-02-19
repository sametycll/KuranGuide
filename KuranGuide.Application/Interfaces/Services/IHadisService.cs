using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Interfaces.Services
{
    public interface IHadisService
    {
        Task<IEnumerable<Hadis>> GetAllAsync();
        Task<Hadis> GetByIdAsync(int id);
        Task<Hadis> CreateAsync(Hadis entity);
        Task<Hadis> UpdateAsync(Hadis entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Hadis>> GetByTemaIdAsync(int temaId);
        Task<IEnumerable<Hadis>> SearchAsync(string text);

        Task<IEnumerable<Hadis>> GetAllWithTemaAsync();
    }
}
