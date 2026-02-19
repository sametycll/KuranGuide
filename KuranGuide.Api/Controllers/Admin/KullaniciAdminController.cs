using KuranGuide.Application.DTOs.Ayet;
using KuranGuide.Application.DTOs.Kullanici;
using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Application.Services;
using KuranGuide.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/kullanici")]
    [Authorize(Roles = "Admin")]
    public class KullaniciAdminController : ControllerBase
    {
        private readonly IKullaniciService _kullaniciService;

        public KullaniciAdminController(IKullaniciService kullaniciService)
        {
            _kullaniciService = kullaniciService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Artık sayıları getiren servisi çağırıyoruz
            var data = await _kullaniciService.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _kullaniciService.GetByIdAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }


        // POST: api/admin/kullanici
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] KullaniciCreateDto dto)
        {
            var entity = new Kullanici
            {
                CreatedAt = DateTime.UtcNow,
                Email = dto.Email,
                PasswordHash = dto.Password,
                Role = dto.Role,
            };

            var created = await _kullaniciService.CreateAsync(entity);
            return Ok(created);
        }

        // PUT: api/admin/ayet/10
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] KullaniciDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id uyuşmuyor.");

            var existing = await _kullaniciService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Email = dto.Email;
            existing.PasswordHash = dto.PasswordHash;
            existing.Role = dto.Role;
            

            var updated = await _kullaniciService.UpdateAsync(existing);      

            return Ok(updated);
        }


        // DELETE: api/admin/ayet/10
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _kullaniciService.DeleteAsync(id);
            if (!result)
                return NotFound("Silinecek kullanıcı bulunamadı.");

            return Ok("Kullanıcı silindi.");
        }



    }
}
