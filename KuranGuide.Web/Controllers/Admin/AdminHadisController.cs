using KuranGuide.Web.Models;
using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KuranGuide.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    //[Route("Admin/[controller]")] 
    [Route("Admin/[controller]/[action]")]
    public class AdminHadisController : AdminBaseController
    {
        private readonly ApiClient _api;

        public AdminHadisController(ApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");
            var hadisler = await _api.GetWithAuthAsync<List<HadisViewModel>>("api/admin/hadis", token);
            return View(hadisler);
        }

        public async Task<IActionResult> Create()
        {
            var temalar = await _api.GetAsync<List<TemaViewModel>>("api/tema");
            ViewBag.Temalar = new SelectList(temalar ?? new List<TemaViewModel>(), "Id", "TemaAdi");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HadisViewModel model)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            if (model.Metin == null || model.Kaynak == null || model.TemaId <1 )
            {
                TempData["Error"] = "Tema eklenemedi. Lütfen bütün alanları eksiksiz doldurunuz!";
                return View(model);
            }

            var result = await _api.PostWithAuthAsync<HadisViewModel, HadisViewModel>("api/admin/hadis", model, token);

            if (result == null)
            {
                TempData["Error"] = "Tema eklenemedi.";
                var temalar = await _api.GetAsync<List<TemaViewModel>>("api/tema");
                ViewBag.Temalar = new SelectList(temalar ?? new List<TemaViewModel>(), "Id", "TemaAdi");
                return View(model);
            }

            TempData["Success"] = "Tema başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            var hadis = await _api.GetWithAuthAsync<HadisViewModel>($"api/admin/hadis/{id}", token);

            var temalar = await _api.GetAsync<List<TemaViewModel>>("api/tema");
            ViewBag.Temalar = new SelectList(temalar ?? new List<TemaViewModel>(), "Id", "TemaAdi");
            return View(hadis);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(HadisViewModel model)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            if (model.Metin == null || model.Kaynak == null || model.TemaId < 1)
            {
                TempData["Error"] = "Tema eklenemedi. Lütfen bütün alanları eksiksiz doldurunuz!";
                return View(model);
            }

            var result = await _api.PutWithAuthAsync<HadisViewModel, HadisViewModel>($"api/admin/hadis/{model.Id}", model, token);

            if (result == null)
            {
                TempData["Error"] = "Güncelleme yapılamadı.";
                var temalar = await _api.GetAsync<List<TemaViewModel>>("api/tema");
                ViewBag.Temalar = new SelectList(temalar ?? new List<TemaViewModel>(), "Id", "TemaAdi");
                return View(model);
            }
            TempData["Success"] = "Tema başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            await _api.DeleteWithAuthAsync($"api/admin/hadis/{id}", token);
            TempData["Success"] = "Tema başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
}
