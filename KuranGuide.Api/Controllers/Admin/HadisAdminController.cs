using KuranGuide.Application.DTOs.Ayet;
using KuranGuide.Application.DTOs.Hadis;
using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Application.Services;
using KuranGuide.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/hadis")]
    [Authorize(Roles = "Admin")]
    public class HadisAdminController : ControllerBase
    {
        private readonly IHadisService _hadisService;

        public HadisAdminController(IHadisService hadisService)
        {
            _hadisService = hadisService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // GetAllAsync YERİNE GetAllWithTemaAsync çağırıyoruz
            var list = await _hadisService.GetAllWithTemaAsync();

            var dto = list.Select(a => new HadisDto
            {
                Id = a.Id,                
                Aciklama = a.Aciklama,
                TemaId = a.TemaId,
                Kaynak = a.Kaynak,
                Metin = a.Metin,
                TemaAciklama = a.Tema?.Aciklama,

                // ARTIK TEMA ADI GELİR (Null check ile)
                TemaAdi = a.Tema != null ? a.Tema?.TemaAdi : "Tema Yok"
            });

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _hadisService.GetByIdAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }


        // POST: api/admin/hadis
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HadisCreateDto dto)
        {
            var entity = new Hadis
            {
                Kaynak = dto.Kaynak,
                Metin = dto.Metin,
                Aciklama = dto.Aciklama,
                TemaId = dto.TemaId
            };

            var created = await _hadisService.CreateAsync(entity);

            var response = new HadisDetailDto
            {
                Id = created.Id,
                Kaynak = created.Kaynak,
                Metin = created.Metin,
                Aciklama = created.Aciklama,
                TemaAdi = null
            };

            return Ok(response);
        }

        // PUT: api/admin/hadis/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] HadisUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id uyuşmuyor.");

            var existing = await _hadisService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Kaynak = dto.Kaynak;
            existing.Metin = dto.Metin;
            existing.Aciklama = dto.Aciklama;
            existing.TemaId = dto.TemaId;

            var updated = await _hadisService.UpdateAsync(existing);

            var response = new HadisDetailDto
            {
                Id = updated.Id,
                Kaynak = updated.Kaynak,
                Metin = updated.Metin,
                Aciklama = updated.Aciklama,
                TemaAdi = null
            };

            return Ok(response);
        }

        // DELETE: api/admin/hadis/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _hadisService.DeleteAsync(id);
            if (!result)
                return NotFound("Silinecek hadis bulunamadı.");

            return Ok("Hadis silindi.");
        }
    }
}
