using KuranGuide.Application.DTOs.Tema;
using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/tema")]
    [Authorize(Roles = "Admin")]
    public class TemaAdminController : ControllerBase
    {
        private readonly ITemaService _temaService;

        public TemaAdminController(ITemaService temaService)
        {
            _temaService = temaService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var data = await _temaService.GetAllAsync();
        //    return Ok(data);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Artık sayıları getiren servisi çağırıyoruz
            var data = await _temaService.GetAllWithCountsAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _temaService.GetByIdAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // POST: api/admin/tema
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TemaCreateDto dto)
        {
            var tema = new Tema
            {
                TemaAdi = dto.TemaAdi,
                Aciklama = dto.Aciklama,
                AnahtarKelimeler = dto.AnahtarKelimeler,
                Icon=dto.Icon
            };

            var created = await _temaService.CreateAsync(tema);

            var response = new TemaDetailDto
            {
                Id = created.Id,
                TemaAdi = created.TemaAdi,
                Aciklama = created.Aciklama,
                AnahtarKelimeler = created.AnahtarKelimeler,
                Icon = created.Icon
            };

            return Ok(response);
        }

        // PUT: api/admin/tema/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TemaUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id uyuşmuyor.");

            var existing = await _temaService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.TemaAdi = dto.TemaAdi;
            existing.Aciklama = dto.Aciklama;
            existing.AnahtarKelimeler = dto.AnahtarKelimeler;
            existing.Icon = dto.Icon;

            var updated = await _temaService.UpdateAsync(existing);

            var response = new TemaDetailDto
            {
                Id = updated.Id,
                TemaAdi = updated.TemaAdi,
                Aciklama = updated.Aciklama,
                AnahtarKelimeler = updated.AnahtarKelimeler,
                Icon=updated.Icon
            };

            return Ok(response);
        }

        // DELETE: api/admin/tema/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _temaService.DeleteAsync(id);
            if (!result)
                return NotFound("Silinecek tema bulunamadı.");

            return Ok("Tema silindi.");
        }

        //[HttpGet]
        //public async Task<IActionResult> GetTemaForAyet(int id)




    }
}
