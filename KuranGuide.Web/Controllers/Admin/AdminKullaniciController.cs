using KuranGuide.Web.Models;
using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KuranGuide.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AdminKullaniciController : AdminBaseController
    {
        private readonly ApiClient _api;

        public AdminKullaniciController(ApiClient api)
        {
            _api = api;
        }
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            var kullanicilar = await _api.GetWithAuthAsync<List<KullaniciViewModel>>("api/admin/kullanici", token);
            return View(kullanicilar);
        }

        public async Task<IActionResult> Create()
        {          
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(KullaniciViewModel model)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            if (model.Email==null || model.Password == null)
            {
                TempData["Error"] = "Kullanıcı eklenemedi. Lütfen bütün alanları doldurunuz!";
                return View(model);
            }

            var result = await _api.PostWithAuthAsync<KullaniciViewModel, KullaniciViewModel>("api/admin/kullanici", model, token);

            if (result == null)
            {
                TempData["Error"] = "Kullanıcı eklenemedi.";     
                return View(model);
            }

            TempData["Success"] = "Kullanıcı başarıyla eklendi.";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            var Kullanici = await _api.GetWithAuthAsync<KullaniciViewModel>($"api/admin/kullanici/{id}", token);   
            return View(Kullanici);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(KullaniciViewModel model)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");
            if (model.Email == null || model.Password == null)
            {
                TempData["Error"] = "Kullanıcı eklenemedi. Lütfen bütün alanları doldurunuz!";
                return View(model);
            }
            var result = await _api.PutWithAuthAsync<KullaniciViewModel, KullaniciViewModel>($"api/admin/kullanici/{model.Id}", model, token);

            if (result == null)
            {
                TempData["Error"] = "Güncelleme yapılamadı.";          
                return View(model);
            }
            TempData["Success"] = "Kullanıcı başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            await _api.DeleteWithAuthAsync($"api/admin/kullanici/{id}", token);
            return RedirectToAction("Index");
        }
    }
}
