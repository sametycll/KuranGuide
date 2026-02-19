using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Application.DTOs.Ayet;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AyetController : ControllerBase
    {
        private readonly IAyetService _ayetService;
        private readonly ITemaService _temaService;
        private readonly ISureService _sureService;
        public AyetController(IAyetService ayetService, ITemaService temaService, ISureService sureService)
        {
            _ayetService = ayetService;
            _temaService = temaService;
            _sureService = sureService;
        }

        // GET: api/ayet
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _ayetService.GetAllAsync();

            var dto = list.Select(a => new AyetDto
            {
                Id = a.Id,
                SureId = a.SureId,
                AyetNo = a.AyetNo,
                Meal = a.Meal
            });

            return Ok(dto);
        }

        // GET: api/ayet/10
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ayet = await _ayetService.GetByIdAsync(id);
            if (ayet == null) return NotFound();

            // 2. Ayetin bağlı olduğu temayı çek (Senin eski kodun)
            // TemaId null ise hata vermemesi için kontrol edelim
            var tema = (ayet.TemaId.HasValue && ayet.TemaId > 0)
                       ? await _temaService.GetByIdAsync(ayet.TemaId.Value)
                       : null;

            // 3. YENİ: Ayetin bağlı olduğu Sureyi çek
            var sure = await _sureService.GetByIdAsync(ayet.SureId);

            var dto = new AyetDetailDto
            {
                Id = ayet.Id,
                SureId = ayet.SureId,

                // 4. Sure Adını DTO'ya eşle
                SureAdi = sure?.SureAdi,

                AyetNo = ayet.AyetNo,
                ArapcaMetin = ayet.ArapcaMetin,
                Meal = ayet.Meal,
                Aciklama = ayet.Aciklama,
                TemaAdi = tema?.TemaAdi,
                TemaId = ayet.TemaId
            };

            return Ok(dto);
        }

        // GET: api/ayet/arama?q=...
        [HttpGet("arama")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Arama metni boş olamaz.");

            var results = await _ayetService.SearchAsync(q);

            var dto = results.Select(a => new AyetSearchResultDto
            {
                Id = a.Id,
                ArapcaMetin = a.ArapcaMetin,
                Meal = a.Meal,
                TemaAdi = a.Tema?.TemaAdi
            });

            return Ok(dto);
        }



        [HttpGet("gunun-ayeti")]
        public async Task<IActionResult> GetDailyAyet()
        {
            // Tek satırda optimize edilmiş veriyi çekiyoruz
            var ayet = await _ayetService.GetGununAyetiAsync();

            if (ayet == null)
                return NotFound("Ayet bulunamadı.");

            // Artık _sureService'i çağırmaya gerek YOK.
            // Çünkü Service/Repository katmanında 'Include' yaptık, SureAdi zaten dolu geldi.

            return Ok(new
            {
                ayet.Id,
                ayet.SureId,
                ayet.AyetNo,
                ayet.ArapcaMetin,
                ayet.Meal,
                // ayet.Aciklama, // DTO'ya eklediysen buraya da ekle
                ayet.TemaId,

                // Servisten dolu geldiği için direkt kullanıyoruz
                SureAdi = ayet.SureAdi
            });
        }

    }
}
