using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Application.DTOs.Favori;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KuranGuide.Api.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    [Authorize]  // Bu controller sadece login olanlara açıktır
    public class FavoriController : ControllerBase
    {
        private readonly IFavoriService _favoriService;
        private readonly IAyetService _ayetService;
        private readonly IHadisService _hadisService;

        public FavoriController(IFavoriService favoriService, IAyetService ayetService, IHadisService hadisService)
        {
            _favoriService = favoriService;
            _ayetService = ayetService;
            _hadisService = hadisService;
        }

        // ---------------------------------------------------------
        // GET api/favori
        // Kullanıcının favorilerini listeler
        // ---------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetUserFavorites()
        {
            int userId = int.Parse(User.FindFirst("id").Value);

            var list = await _favoriService.GetUserFavoritesAsync(userId);

            var result = new List<FavoriDto>();

            foreach (var f in list)
            {
                var dto = new FavoriDto
                {
                    Id = f.Id,
                    Type = f.Type,
                    RefId = f.RefId,
                    CreatedAt = DateTime.UtcNow
                };

                if (f.Type == "ayet")
                {
                    var ayet = await _ayetService.GetByIdAsync(f.RefId);

                    if (ayet != null)
                    {
                        dto.TextPreview = ayet.Meal.Length > 120
                            ? ayet.Meal.Substring(0, 120) + "..."
                            : ayet.Meal;

                        dto.SourceInfo = $"{ayet.SureId}. Sûre, {ayet.AyetNo}. Ayet";
                    }
                }
                else if (f.Type == "hadis")
                {
                    var hadis = await _hadisService.GetByIdAsync(f.RefId);

                    if (hadis != null)
                    {
                        dto.TextPreview = hadis.Metin.Length > 120
                            ? hadis.Metin.Substring(0, 120) + "..."
                            : hadis.Metin;

                        dto.SourceInfo = hadis.Kaynak;
                    }
                }

                result.Add(dto);
            }

            return Ok(result);
        }

        // ---------------------------------------------------------
        // POST api/favori
        // Yeni favori ekleme
        // ---------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] AddFavoriDto model)
        {
            if (model.Type != "ayet" && model.Type != "hadis")
                return BadRequest("Type yalnızca 'ayet' veya 'hadis' olabilir.");

            int userId = int.Parse(User.FindFirst("id").Value);

            var favori = new Domain.Entities.Favori
            {
                UserId = userId,
                Type = model.Type,
                RefId = model.RefId,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _favoriService.AddFavoriteAsync(favori);

            return Ok(new FavoriDto
            {
                Id = created.Id,
                Type = created.Type,
                RefId = created.RefId,
                CreatedAt = DateTime.UtcNow
            });
        }

        // ---------------------------------------------------------
        // DELETE api/favori/{id}
        // Belirli bir favoriyi silme
        // ---------------------------------------------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = int.Parse(User.FindFirst("id").Value);

            bool removed = await _favoriService.RemoveFavoriteAsync(id, userId);

            if (!removed)
                return BadRequest("Favori bulunamadı veya bu kullanıcıya ait değil.");

            return Ok("Favori silindi.");
        }



        // POST api/favori/toggle
        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle([FromBody] AddFavoriDto model)
        {
            int userId = int.Parse(User.FindFirst("id").Value);

            var result = await _favoriService.ToggleFavoriteAsync(userId, model.Type, model.RefId);

            return Ok(result);
        }











    }
}
