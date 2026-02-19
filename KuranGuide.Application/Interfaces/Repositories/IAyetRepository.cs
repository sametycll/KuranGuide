using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Interfaces.Repositories
{
    public interface IAyetRepository : IGenericRepository<Ayet> {
        Task<List<Ayet>> GetAllWithTemaAsync();
        Task<Ayet> GetGununAyetiAsync();
    }
}
