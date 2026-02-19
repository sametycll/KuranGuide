using KuranGuide.Web.Models;
using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Web.Controllers
{
    public class SureController : Controller
    {
        private readonly ApiClient _api;

        public SureController(ApiClient api)
        {
            _api = api;
        }

        // Sure Listesi
        public async Task<IActionResult> Index()
        {
            var sureler = await _api.GetAsync<List<SureListeViewModel>>("api/sure");
            return View(sureler);
        }

        // Sure Detayı (Okuma Ekranı)
        public async Task<IActionResult> Detay(int id)
        {
            // id parametresi burada SureNo (1..114) olarak kullanılmalı
            // API'deki endpoint: api/sure/{sureNo}
            var sure = await _api.GetAsync<SureDetayViewModel>($"api/sure/{id}");

            if (sure == null) return RedirectToAction("Index");

            return View(sure);
        }
    }
}