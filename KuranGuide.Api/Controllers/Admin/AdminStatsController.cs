using KuranGuide.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/stats")]
    [Authorize(Roles = "Admin")]
    public class AdminStatsController : ControllerBase
    {
        private readonly ITemaService _temaService;
        private readonly IAyetService _ayetService;
        private readonly IHadisService _hadisService;

        public AdminStatsController(ITemaService temaService, IAyetService ayetService, IHadisService hadisService)
        {
            _temaService = temaService;
            _ayetService = ayetService;
            _hadisService = hadisService;
        }

        [HttpGet("tema-count")]
        public async Task<int> GetTemaCount()
        {
            var list = await _temaService.GetAllAsync();
            return list.Count();
        }

        [HttpGet("ayet-count")]
        public async Task<int> GetAyetCount()
        {
            var list = await _ayetService.GetAllAsync();
            return list.Count();
        }

        [HttpGet("hadis-count")]
        public async Task<int> GetHadisCount()
        {
            var list = await _hadisService.GetAllAsync();
            return list.Count();
        }

        [HttpGet("latest-ayetler")]
        public async Task<IEnumerable<object>> LatestAyetler()
        {
            var ayetler = await _ayetService.GetAllAsync();
            return ayetler
                .OrderByDescending(a => a.Id)
                .Take(5)
                .Select(a => new {
                    a.Id,
                    a.SureId,
                    a.AyetNo,
                    Meal = a.Meal.Length > 80 ? a.Meal[..80] + "..." : a.Meal
                });
        }

        [HttpGet("latest-hadisler")]
        public async Task<IEnumerable<object>> LatestHadisler()
        {
            var hadisler = await _hadisService.GetAllAsync();
            return hadisler
                .OrderByDescending(h => h.Id)
                .Take(5)
                .Select(h => new {
                    h.Id,
                    Metin = h.Metin.Length > 80 ? h.Metin[..80] + "..." : h.Metin,
                    h.Kaynak
                });
        }
    }
}
