using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Domain.Entities;
using KuranGuide.Infrastructure.Context;

namespace KuranGuide.Infrastructure.Repositories
{
    public class KullaniciRepository : GenericRepository<Kullanici>, IKullaniciRepository
    {
        public KullaniciRepository(KuranGuideDbContext context) : base(context)
        {
        }
    }
}
