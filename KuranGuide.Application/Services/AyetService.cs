using KuranGuide.Application.DTOs.Ayet;
using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Services
{
    public class AyetService : IAyetService
    {
        private readonly IAyetRepository _repo;

        public AyetService(IAyetRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Ayet>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Ayet> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Ayet> CreateAsync(Ayet entity)
        {
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return entity;
        }

        public async Task<Ayet> UpdateAsync(Ayet entity)
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

        public async Task<IEnumerable<Ayet>> GetByTemaIdAsync(int temaId)
        {
            return await _repo.FindAsync(a => a.TemaId == temaId);
        }

        public async Task<IEnumerable<Ayet>> SearchAsync(string text)
        {
            return await _repo.FindAsync(a =>
                a.Meal.Contains(text) ||
                a.ArapcaMetin.Contains(text) ||
                a.Aciklama.Contains(text)
            );
        }

        public async Task<IEnumerable<Ayet>> GetAllWithTemaAsync()
        {
            // Repository'deki Include'lu metodu çağırıyoruz
            return await _repo.GetAllWithTemaAsync();
        }

        public async Task<AyetDto> GetGununAyetiAsync()
        {
            // Repository'deki optimize metodu çağır
            var ayet = await _repo.GetGununAyetiAsync();

            if (ayet == null) return null;

            // Entity -> DTO (SureAdi artık ayet.Sure.SureAdi içinde var)
            return new AyetDto
            {
                Id = ayet.Id,
                SureId = ayet.SureId,
                SureAdi = ayet.Sure?.SureAdi, // Include ile geldiği için dolu!
                AyetNo = ayet.AyetNo,
                ArapcaMetin = ayet.ArapcaMetin,
                Meal = ayet.Meal,
                TemaId = ayet.TemaId,
                Aciklama = ayet.Aciklama,
                TemaAdi = ayet.Tema?.TemaAdi


            };
        }

      
    }
}
