using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Application.DTOs.Tema;
using KuranGuide.Application.DTOs.Ayet;
using KuranGuide.Application.DTOs.Hadis;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemaController : ControllerBase
    {
        private readonly ITemaService _temaService;
        private readonly IAyetService _ayetService;
        private readonly IHadisService _hadisService;

        public TemaController(
            ITemaService temaService,
            IAyetService ayetService,
            IHadisService hadisService)
        {
            _temaService = temaService;
            _ayetService = ayetService;
            _hadisService = hadisService;
        }

        // GET: api/tema
        [HttpGet]
        [ResponseCache(Duration = 300)] // 5 dakika cache
        public async Task<IActionResult> GetAll()
        {
            var temalar = await _temaService.GetAllAsync();

            var dto = temalar.Select(t => new TemaDto
            {
                Id = t.Id,
                TemaAdi = t.TemaAdi,
                Aciklama = t.Aciklama,
                Icon = t.Icon
            });

            return Ok(dto);
        }

        // GET: api/tema/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tema = await _temaService.GetByIdAsync(id);
            if (tema == null)
                return NotFound();

            var dto = new TemaDetailDto
            {
                Id = tema.Id,
                TemaAdi = tema.TemaAdi,
                Aciklama = tema.Aciklama,
                AnahtarKelimeler = tema.AnahtarKelimeler,
                Icon = tema.Icon
            };

            return Ok(dto);
        }

        // GET: api/tema/5/ayetler
        [HttpGet("{id:int}/ayetler")]
        public async Task<IActionResult> GetAyetler(int id)
        {
            var ayetler = await _ayetService.GetByTemaIdAsync(id);

            var dto = ayetler.Select(a => new AyetDto
            {
                Id = a.Id,
                SureId = a.SureId,
                AyetNo = a.AyetNo,
                ArapcaMetin = a.ArapcaMetin,
                Meal = a.Meal
            });

            return Ok(dto);
        }

        // GET: api/tema/5/hadisler
        [HttpGet("{id:int}/hadisler")]
        public async Task<IActionResult> GetHadisler(int id)
        {
            var hadisler = await _hadisService.GetByTemaIdAsync(id);

            var dto = hadisler.Select(h => new HadisDto
            {
                Id = h.Id,
                Kaynak = h.Kaynak,
                Metin = h.Metin
            });

            return Ok(dto);
        }
    }
}
