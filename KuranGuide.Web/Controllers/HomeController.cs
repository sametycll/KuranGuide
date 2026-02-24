using KuranGuide.Application.DTOs.General;
using KuranGuide.Web.Models;
using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _api;

        public HomeController(ApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            AyetViewModel gununAyeti = null;
            HadisViewModel gununHadisi = null;
            List<TemaViewModel> temalar = null;

            try
            {
                // API çağrıları
                gununAyeti = await _api.GetAsync<AyetViewModel>("api/ayet/gunun-ayeti");
                gununHadisi = await _api.GetAsync<HadisViewModel>("api/hadis/gunun-hadisi");
                temalar = await _api.GetAsync<List<TemaViewModel>>("api/tema");
            }
            catch
            {
                // API kapalıysa veya hata varsa program patlamasın, null devam etsin
            }

            // 3. Favori Kontrolü
            bool isFavoritedHadis = false;
            bool isFavoritedAyet = false;
            var token = Request.Cookies["jwt"];

            if (token != null)
            {
                try
                {
                    var favs = await _api.GetWithAuthAsync<List<FavoriViewModel>>("api/favori", token);
                    isFavoritedHadis = favs?.Any(x => x.Type == "hadis" && x.RefId == gununHadisi.Id) ?? false;
                    isFavoritedAyet = favs?.Any(x => x.Type == "ayet" && x.RefId == gununAyeti.Id) ?? false;
                }
                catch
                {
                    // Token hatası olursa akışı bozma
                    isFavoritedHadis = false;
                    isFavoritedAyet = false;
                }
            }

            var model = new HomePageViewModel
            {
                GununAyeti = gununAyeti,
                GununHadisi = gununHadisi,
                IsFavoritedHadis = isFavoritedHadis,
                IsFavoritedAyet = isFavoritedAyet,
                // Eğer temalar null ise boş liste ata ki foreach döngüsü patlamasın
                PopulerTemalar = temalar?.Take(5).ToList() ?? new List<TemaViewModel>()
            };

            return View(model);
        }



        [HttpGet]
        [Route("Arama")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return RedirectToAction("Index");

            // URL'yi güvenli hale getir (boşlukları %20 yapar vs.)
            var encodedQ = Uri.EscapeDataString(q);

            // API çağrısı
            var results = await _api.GetAsync<SearchResponseDto>($"api/search?q={encodedQ}");

            ViewBag.Query = q;

            // Eğer results null gelirse hata vermesin diye yeni obje oluştur
            return View(results ?? new SearchResponseDto());
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();

        }
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();

        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();

        }

        //mail gönderme işlemi simülasyonu
        [HttpPost]
        public IActionResult SendContact(string FullName, string Email, string Subject, string Message)
        {
            // Burada mail gönderme işlemi yapılabilir.
            TempData["Success"] = "Mesajınız bize ulaştı. Teşekkür ederiz!";
            return RedirectToAction("Contact");
        }

        [Route("Home/Error/{statusCode?}")]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode == 404)
            {
                return View("Error404");
            }
            return View();
        }

    }
}
