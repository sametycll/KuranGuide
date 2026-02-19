using KuranGuide.Application.DTOs.Tema;
using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Interfaces.Repositories
{
    public interface ITemaRepository : IGenericRepository<Tema>
    {
        Task<List<TemaDto>> GetAllWithCountsAsync();
    }
}
