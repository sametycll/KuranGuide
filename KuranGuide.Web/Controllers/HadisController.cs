using KuranGuide.Web.Models;
using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Web.Controllers
{
    public class HadisController : Controller
    {
        private readonly ApiClient _api;

        public HadisController(ApiClient api)
        {
            _api = api;
        }


        public async Task<IActionResult> Index()
        {
            // API'den tüm hadisleri çekiyoruz
            var hadisler = await _api.GetAsync<List<HadisViewModel>>("api/hadis");

            // Eğer veri yoksa boş liste gönder
            return View(hadisler ?? new List<HadisViewModel>());
        }


        public async Task<IActionResult> Detay(int id)
        {
            // 1. Hadis'i getir
            var hadis = await _api.GetAsync<HadisViewModel>($"api/hadis/{id}");

            if (hadis == null)
                return RedirectToAction("Index", "Home");

            // 2. Tema Kontrolü (HATA BURADAYDI)
            // TemaId null ise API'ye hiç gitme, çünkü gidersen "GetAll" çalışır ve liste döner.
            TemaViewModel? tema = null;

            if (hadis.TemaId.HasValue && hadis.TemaId.Value > 0)
            {
                // Sadece ID varsa istek at, böylece tek obje döner
                tema = await _api.GetAsync<TemaViewModel>($"api/tema/{hadis.TemaId}");
            }

            // 3. Favori Kontrolü
            bool isFavorited = false;
            var token = Request.Cookies["jwt"];

            if (token != null)
            {
                try
                {
                    var favs = await _api.GetWithAuthAsync<List<FavoriViewModel>>("api/favori", token);
                    isFavorited = favs?.Any(x => x.Type == "hadis" && x.RefId == id) ?? false;
                }
                catch
                {
                    // Token hatası olursa akışı bozma
                    isFavorited = false;
                }
            }

            // 4. Modeli oluştur
            var model = new HadisDetailViewModel
            {
                Hadis = hadis,
                Tema = tema, // Tema yoksa burası null gider, View'da if kontrolü yapmalısın
                IsFavorited = isFavorited
            };

            return View(model);
        }

    }
}
