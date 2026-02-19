using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Services
{
    public class HadisService : IHadisService
    {
        private readonly IHadisRepository _repo;

        public HadisService(IHadisRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Hadis>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Hadis> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Hadis> CreateAsync(Hadis entity)
        {
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return entity;
        }

        public async Task<Hadis> UpdateAsync(Hadis entity)
        {
            await _repo.UpdateAsync(entity);
            await _repo.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Hadis>> GetByTemaIdAsync(int temaId)
        {
            return await _repo.FindAsync(h => h.TemaId == temaId);
        }

        public async Task<IEnumerable<Hadis>> SearchAsync(string text)
        {
            return await _repo.FindAsync(h =>
                h.Metin.Contains(text) ||
                h.Aciklama.Contains(text)
            );
        }

        public async Task<IEnumerable<Hadis>> GetAllWithTemaAsync()
        {
            // Repository'deki Include'lu metodu çağırıyoruz
            return await _repo.GetAllWithTemaAsync();
        }
    }
}
