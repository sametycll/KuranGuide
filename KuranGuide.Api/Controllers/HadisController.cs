using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Application.DTOs.Hadis;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HadisController : ControllerBase
    {
        private readonly IHadisService _hadisService;
        private readonly ITemaService _temaService;

        public HadisController(IHadisService hadisService, ITemaService temaService)
        {
            _hadisService = hadisService;
            _temaService = temaService;
        }

        // GET: api/hadis
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _hadisService.GetAllAsync();

            var dto = list.Select(h => new HadisDto
            {
                Id = h.Id,
                Kaynak = h.Kaynak,
                Metin = h.Metin
            });

            return Ok(dto);
        }

        // GET: api/hadis/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hadis = await _hadisService.GetByIdAsync(id);
            if (hadis == null)
                return NotFound();

            var tema = await _temaService.GetByIdAsync(hadis.TemaId);

            var dto = new HadisDetailDto
            {
                Id = hadis.Id,
                Kaynak = hadis.Kaynak,
                Metin = hadis.Metin,
                Aciklama = hadis.Aciklama,
                TemaAdi = tema?.TemaAdi,
                TemaId = hadis.TemaId
            };

            return Ok(dto);
        }

        [HttpGet("gunun-hadisi")]
        public async Task<IActionResult> GetDailyHadis()
        {
            var all = (await _hadisService.GetAllAsync()).ToList();

            if (!all.Any())
                return NotFound("Hadis bulunamadı.");

            int index = DateTime.UtcNow.DayOfYear % all.Count;

            var hadis = all[index];

            return Ok(new
            {
                hadis.Id,
                hadis.Metin,
                hadis.Kaynak,
                hadis.Aciklama,
                hadis.TemaId
            });
        }



        // GET: api/hadis/arama?q=...
        [HttpGet("arama")]
        public async Task<IActionResult> Search(string q)
        {
            var results = await _hadisService.SearchAsync(q);

            var dto = results.Select(h => new HadisSearchResultDto
            {
                Id = h.Id,
                Metin = h.Metin,
                Kaynak = h.Kaynak,
                TemaAdi = h.Tema?.TemaAdi
            });

            return Ok(dto);
        }
    }
}
