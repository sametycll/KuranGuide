using KuranGuide.Application.DTOs.Ayet;
using KuranGuide.Application.DTOs.Sure;
using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Services
{
    public class SureService : ISureService
    {
        private readonly ISureRepository _sureRepo;

        public SureService(ISureRepository sureRepo)
        {
            _sureRepo = sureRepo;
        }

        public async Task<IEnumerable<SureDto>> GetAllAsync()
        {
            var list = await _sureRepo.GetAllAsync();
            // Entity -> DTO dönüşümü (AutoMapper kullanılabilir, burada manuel yapıyoruz)
            return list.OrderBy(x => x.SureNo).Select(s => new SureDto
            {
                Id = s.Id,
                SureNo = s.SureNo,
                SureAdi = s.SureAdi,
                ArapcaAdi = s.ArapcaAdi,
                AyetSayisi = s.AyetSayisi,
                Yer = s.Yer
            });
        }

        public async Task<SureDetailDto> GetSureDetailAsync(int sureNo)
        {
            var sure = await _sureRepo.GetBySureNoWithAyetlerAsync(sureNo);
            if (sure == null) return null;

            return new SureDetailDto
            {
                Id = sure.Id,
                SureNo = sure.SureNo,
                SureAdi = sure.SureAdi,
                ArapcaAdi = sure.ArapcaAdi,
                Yer = sure.Yer,
                Ayetler = sure.Ayetler.OrderBy(a => a.AyetNo).Select(a => new AyetDto
                {
                    Id = a.Id,
                    SureId = sure.SureNo,
                    AyetNo = a.AyetNo,
                    ArapcaMetin = a.ArapcaMetin,
                    Meal = a.Meal,
                    // TemaAdi serviste join yapılmadıysa boş gelebilir, önemli değil
                }).ToList()
            };
        }



        // --- YENİ EKLENEN METOT ---
        public async Task<SureDto> GetByIdAsync(int id)
        {
            // Repository'den entity'i çekiyoruz
            var sure = await _sureRepo.GetByIdAsync(id);

            if (sure == null) return null;

            // Entity -> DTO Dönüşümü
            return new SureDto
            {
                Id = sure.Id,
                SureNo = sure.SureNo,
                SureAdi = sure.SureAdi,
                ArapcaAdi = sure.ArapcaAdi,
                AyetSayisi = sure.AyetSayisi,
                Yer = sure.Yer
            };
        }



    }
}