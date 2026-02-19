using KuranGuide.Application.DTOs.Ayet;
using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Application.Services;
using KuranGuide.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/ayet")]
    [Authorize(Roles = "Admin")]
    public class AyetAdminController : ControllerBase
    {
        private readonly IAyetService _ayetService;

        public AyetAdminController(IAyetService ayetService)
        {
            _ayetService = ayetService;
        }

        // GET: api/ayet
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // GetAllAsync YERİNE GetAllWithTemaAsync çağırıyoruz
            var list = await _ayetService.GetAllWithTemaAsync();

            var dto = list.Select(a => new AyetDto
            {
                Id = a.Id,
                SureId = a.SureId,
                AyetNo = a.AyetNo,
                ArapcaMetin = a.ArapcaMetin,
                Meal = a.Meal,
                Aciklama = a.Aciklama,
                TemaId = a.TemaId,

                // ARTIK TEMA ADI GELİR (Null check ile)
                TemaAdi = a.Tema != null ? a.Tema.TemaAdi : "Tema Yok"
            });

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _ayetService.GetByIdAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }


        // POST: api/admin/ayet
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AyetCreateDto dto)
        {
            var entity = new Ayet
            {
                SureId = dto.SureId,
                AyetNo = dto.AyetNo,
                ArapcaMetin = dto.ArapcaMetin,
                Meal = dto.Meal,
                Aciklama = dto.Aciklama,
                TemaId = dto.TemaId
            };

            var created = await _ayetService.CreateAsync(entity);

            var response = new AyetDetailDto
            {
                Id = created.Id,
                SureId = created.SureId,
                AyetNo = created.AyetNo,
                ArapcaMetin = created.ArapcaMetin,
                Meal = created.Meal,
                Aciklama = created.Aciklama,
                TemaAdi = null // İstersen TemaService ile çekip doldurabilirsin
            };

            return Ok(response);
        }

        // PUT: api/admin/ayet/10
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AyetUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id uyuşmuyor.");

            var existing = await _ayetService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.SureId = dto.SureId;
            existing.AyetNo = dto.AyetNo;
            existing.ArapcaMetin = dto.ArapcaMetin;
            existing.Meal = dto.Meal;
            existing.Aciklama = dto.Aciklama;
            existing.TemaId = dto.TemaId;

            var updated = await _ayetService.UpdateAsync(existing);

            var response = new AyetDetailDto
            {
                Id = updated.Id,
                SureId = updated.SureId,
                AyetNo = updated.AyetNo,
                ArapcaMetin = updated.ArapcaMetin,
                Meal = updated.Meal,
                Aciklama = updated.Aciklama,
                TemaAdi = null
            };

            return Ok(response);
        }

        // DELETE: api/admin/ayet/10
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ayetService.DeleteAsync(id);
            if (!result)
                return NotFound("Silinecek ayet bulunamadı.");

            return Ok("Ayet silindi.");
        }
    }
}
