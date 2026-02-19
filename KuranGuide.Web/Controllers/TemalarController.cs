using KuranGuide.Web.Models;
using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Mvc;

public class TemalarController : Controller
{
    private readonly ApiClient _api;

    public TemalarController(ApiClient api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var temalar = await _api.GetAsync<List<TemaViewModel>>("api/tema");

        // Eğer temalar null ise boş liste oluştur
        if (temalar == null)
        {
            return View(new List<TemaViewModel>());
        }
        // Ayet ve hadis sayısını her tema için alalım
        foreach (var tema in temalar)
        {
            var ayetler = await _api.GetListAsync<AyetViewModel>($"api/tema/{tema.Id}/ayetler");
            var hadisler = await _api.GetListAsync<HadisViewModel>($"api/tema/{tema.Id}/hadisler");

            tema.AyetSayisi = ayetler.Count;
            tema.HadisSayisi = hadisler.Count;
        }

        // A-Z Sıralama Yap
        var siraliTemalar = temalar.OrderBy(x => x.TemaAdi).ToList();
        return View(siraliTemalar);
    }


    public async Task<IActionResult> Detay(int id)
    {
        // Tema bilgisi
        var tema = await _api.GetAsync<TemaViewModel>($"api/tema/{id}");

        // Tema'ya bağlı ayetler ve hadisler
        var ayetler = await _api.GetListAsync<AyetViewModel>($"api/tema/{id}/ayetler");
        var hadisler = await _api.GetListAsync<HadisViewModel>($"api/tema/{id}/hadisler");


        var model = new TemaDetailViewModel
        {
            Tema = tema,
            Ayetler = ayetler ?? new List<AyetViewModel>(),
            Hadisler = hadisler ?? new List<HadisViewModel>(),
            
        };

        return View(model);
    }








}
