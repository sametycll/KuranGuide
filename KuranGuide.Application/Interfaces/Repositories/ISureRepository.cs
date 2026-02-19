using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Interfaces.Repositories
{
    public interface ISureRepository : IGenericRepository<Sure>
    {
        Task<Sure> GetBySureNoWithAyetlerAsync(int sureNo);
    }
}