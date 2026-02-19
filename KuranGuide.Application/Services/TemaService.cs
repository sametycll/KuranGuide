using KuranGuide.Application.DTOs.Tema;
using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Services
{
    public class TemaService : ITemaService
    {
        private readonly ITemaRepository _repo;

        public TemaService(ITemaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Tema>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Tema> GetByIdAsync(int? id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Tema> CreateAsync(Tema tema)
        {
            await _repo.AddAsync(tema);
            await _repo.SaveChangesAsync();
            return tema;
        }

        public async Task<Tema> UpdateAsync(Tema tema)
        {
            await _repo.UpdateAsync(tema);
            await _repo.SaveChangesAsync();
            return tema;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(entity);
            await _repo.SaveChangesAsync();
            return true;
        }
        public async Task<List<TemaDto>> GetAllWithCountsAsync()
        {
            return await _repo.GetAllWithCountsAsync();
        }

    }
}
