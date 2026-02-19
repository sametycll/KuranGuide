using KuranGuide.Web.Models;
using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    //[Route("Admin/[controller]")] 
    [Route("Admin/[controller]/[action]")]
    public class AdminTemaController : AdminBaseController
    {
        private readonly ApiClient _api;

        public AdminTemaController(ApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");
            var temalar = await _api.GetWithAuthAsync<List<TemaViewModel>>("api/admin/tema",token);

            return View(temalar);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TemaViewModel model)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            if (model.TemaAdi == null || model.Aciklama == null || model.AnahtarKelimeler == null)
            {
                TempData["Error"] = "Tema eklenemedi. Lütfen bütün alanları doldurunuz!";
                return View(model);
            }

            model.Icon = model.Icon ?? "ph-hash";
          
            var result = await _api.PostWithAuthAsync<TemaViewModel, TemaViewModel>("api/admin/tema", model,token);

            if (result == null)
            {
                TempData["Error"] = "Tema eklenemedi.";
                return View(model);
            }

            TempData["Success"] = "Tema başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            var tema = await _api.GetWithAuthAsync<TemaViewModel>($"api/admin/tema/{id}", token);
          
            return View(tema);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TemaViewModel model)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            if (model.TemaAdi == null || model.Aciklama == null || model.AnahtarKelimeler == null)
            {
                TempData["Error"] = "Tema eklenemedi. Lütfen bütün alanları doldurunuz!";
                return View(model);
            }
            model.Icon = model.Icon ?? "ph-hash";
            var result = await _api.PutWithAuthAsync<TemaViewModel, TemaViewModel>($"api/admin/tema/{model.Id}", model,token);

            if (result == null)
            {
                TempData["Error"] = "Güncelleme yapılamadı.";
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

            await _api.DeleteWithAuthAsync($"api/admin/tema/{id}",token);
            return RedirectToAction("Index");
        }
    }
}
