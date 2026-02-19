using KuranGuide.Application.DTOs.Tema;
using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Interfaces.Services
{
    public interface ITemaService
    {
        Task<IEnumerable<Tema>> GetAllAsync();
        Task<Tema> GetByIdAsync(int? id);
        Task<Tema> CreateAsync(Tema tema);
        Task<Tema> UpdateAsync(Tema tema);
        Task<bool> DeleteAsync(int id);

        Task<List<TemaDto>> GetAllWithCountsAsync();
    }
}
