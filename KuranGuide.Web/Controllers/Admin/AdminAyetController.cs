using KuranGuide.Web.Models;
using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KuranGuide.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AdminAyetController : AdminBaseController
    {
        private readonly ApiClient _api;

        public AdminAyetController(ApiClient api)
        {
            _api = api;
        }
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            var ayetler = await _api.GetWithAuthAsync<List<AyetViewModel>>("api/admin/ayet", token);
            return View(ayetler);
        }

        public async Task<IActionResult> Create()
        {
            var temalar = await _api.GetAsync<List<TemaViewModel>>("api/tema");
            ViewBag.Temalar = new SelectList(temalar ?? new List<TemaViewModel>(), "Id", "TemaAdi");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AyetViewModel model)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            if (model.TemaId ==0 || model.SureId < 1 || model.AyetNo < 1 || model.ArapcaMetin == null || model.Meal ==null || model.Aciklama == null )
            {
                TempData["Error"] = "Ayet eklenemedi. Lütfen bütün alanları eksiksiz doldurun!";
                return View(model);
            }

            var result = await _api.PostWithAuthAsync<AyetViewModel, AyetViewModel>("api/admin/ayet", model, token);

            if (result == null)
            {
                TempData["Error"] = "Ayet eklenemedi.";
                var temalar = await _api.GetAsync<List<TemaViewModel>>("api/tema");
                ViewBag.Temalar = new SelectList(temalar ?? new List<TemaViewModel>(), "Id", "TemaAdi");
                return View(model);
            }

            TempData["Success"] = "Ayet başarıyla eklendi.";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            var ayet = await _api.GetWithAuthAsync<AyetViewModel>($"api/admin/ayet/{id}", token);
            var temalar = await _api.GetAsync<List<TemaViewModel>>("api/tema");
            ViewBag.Temalar = new SelectList(temalar ?? new List<TemaViewModel>(), "Id", "TemaAdi");
            return View(ayet);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AyetViewModel model)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");


            if (model.TemaId == 0 || model.SureId < 1 || model.AyetNo < 1 || model.ArapcaMetin == null || model.Meal == null || model.Aciklama == null)
            {
                TempData["Error"] = "Ayet eklenemedi. Lütfen bütün alanları eksiksiz doldurun!";
                return View(model);
            }

            var result = await _api.PutWithAuthAsync<AyetViewModel, AyetViewModel>($"api/admin/ayet/{model.Id}", model, token);

            if (result == null)
            {
                TempData["Error"] = "Güncelleme yapılamadı.";
                var temalar = await _api.GetAsync<List<TemaViewModel>>("api/tema");
                ViewBag.Temalar = new SelectList(temalar ?? new List<TemaViewModel>(), "Id", "TemaAdi");
                return View(model);
            }
            TempData["Success"] = "Ayet başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var token = Request.Cookies["jwt"];
            if (token == null)
                return RedirectToAction("Login", "Auth");

            await _api.DeleteWithAuthAsync($"api/admin/ayet/{id}", token);
            return RedirectToAction("Index");
        }




    }
}
