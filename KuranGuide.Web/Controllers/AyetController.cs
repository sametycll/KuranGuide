using KuranGuide.Web.Models;
using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Web.Controllers
{
    public class AyetController : Controller
    {
        private readonly ApiClient _api;

        public AyetController(ApiClient api)
        {
            _api = api;
        }        
        public async Task<IActionResult> Detay(int id)
        {
            // 1. Ayeti Getir
            var ayet = await _api.GetAsync<AyetViewModel>($"api/ayet/{id}");

            if (ayet == null)
                return RedirectToAction("Index", "Home");

            // 2. Tema Kontrolü (TemaId var mı?)
            TemaViewModel tema = null;

            // Eğer TemaId null değilse ve 0'dan büyükse API'ye sor
            if (ayet.TemaId.HasValue && ayet.TemaId.Value > 0)
            {
                tema = await _api.GetAsync<TemaViewModel>($"api/tema/{ayet.TemaId}");
            }

            // 3. Favori Kontrolü
            bool isFavorited = false;
            var token = Request.Cookies["jwt"];

            if (token != null)
            {
                try
                {
                    var favs = await _api.GetWithAuthAsync<List<FavoriViewModel>>("api/favori", token);
                    isFavorited = favs?.Any(x => x.Type == "ayet" && x.RefId == id) ?? false;
                }
                catch
                {
                    // Token süresi dolmuşsa veya API hatası varsa favori false kalsın, akış bozulmasın.
                    isFavorited = false;
                }
            }

            // 4. Modeli Oluştur
            var model = new AyetDetailViewModel
            {
                Ayet = ayet,
                Tema = tema, // Tema yoksa burası null gider, View'da kontrol etmelisin
                IsFavorited = isFavorited,
               
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Arama(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return RedirectToAction("Index", "Home");

            // API'de arama endpoint'i: api/ayet/arama?q=...
            var sonuclar = await _api.GetAsync<List<AyetViewModel>>($"api/ayet/arama?q={q}");

            var model = new AramaViewModel
            {
                Sorgu = q,
                Sonuclar = sonuclar ?? new List<AyetViewModel>()
            };

            return View(model);
        }


        //indir butonu koyup resim şeklinde galeriye kaydettireceğim.
        //hakkımızda gizlilik iletişim sayfalarını yapalım

    }
}
