using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Mvc;
using KuranGuide.Web.Models;
using KuranGuide.Application.DTOs.Favori;
using KuranGuide.Domain.Entities;

namespace KuranGuide.Web.Controllers
{
    public class FavoriController : Controller
    {
        private readonly ApiClient _api;

        public FavoriController(ApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["jwt"];

            if (token == null)
                return RedirectToAction("Login", "Auth");

            var items = await _api.GetWithAuthAsync<List<FavoriViewModel>>("api/favori", token)
             ?? new List<FavoriViewModel>();


            var model = new FavoriPageViewModel
            {
                Ayetler = items.Where(x => x.Type == "ayet").ToList(),
                Hadisler = items.Where(x => x.Type == "hadis").ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var token = Request.Cookies["jwt"];

            await _api.DeleteWithAuthAsync($"api/favori/{id}", token);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AjaxToggle([FromBody] AddFavoriDto dto)
        {
            var token = Request.Cookies["jwt"];

            // Login değilse login sayfasına yönlendir
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { redirect = Url.Action("Login", "Auth") });
            }

            // Kullanıcının mevcut favorilerini çek
            var favs = await _api.GetWithAuthAsync<List<FavoriViewModel>>("api/favori", token);
            var existing = favs?.FirstOrDefault(x => x.Type == dto.Type && x.RefId == dto.RefId);

            // ZATEN FAVORİ → SİL
            if (existing != null)
            {
                await _api.DeleteWithAuthAsync($"api/favori/{existing.Id}", token);
                return Json(new { added = false });
            }

            // FAVORİ DEĞİL → EKLE
            var body = new
            {
                Type = dto.Type,
                RefId = dto.RefId
            };

            var result = await _api.PostWithAuthAsync<AddFavoriDto, ToggleFavoriResult>("api/favori/toggle",dto,token);

            return Json(result);
        }



    }
}
