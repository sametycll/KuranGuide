using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Interfaces.Repositories
{
    public interface IHadisRepository : IGenericRepository<Hadis> {
        Task<List<Hadis>> GetAllWithTemaAsync();
    }
}
