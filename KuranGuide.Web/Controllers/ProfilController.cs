using KuranGuide.Web.Models;
using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace KuranGuide.Web.Controllers
{
    public class ProfilController : Controller
    {
        private readonly ApiClient _api;

        public ProfilController(ApiClient api)
        {
            _api = api;
        }

        public IActionResult Index()
        {
            var jwt = Request.Cookies["jwt"];
            if (jwt == null) return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpGet]
        public IActionResult SifreDegistir()
        {
            var jwt = Request.Cookies["jwt"];
            if (jwt == null) return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SifreDegistir(ChangePasswordViewModel model)
        {
            var jwt = Request.Cookies["jwt"];
            if (jwt == null) return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // API'ye sadece yeni şifreyi gönderiyoruz
            var payload = new
            {
                NewPassword = model.NewPassword
            };

            // API endpoint'inin buna uygun olması lazım (Aşağıda API kodunu da verdim)
            var response = await _api.PostWithAuthAsync<object, dynamic>("api/auth/reset-password-direct", payload, jwt);

            if (response != null)
            {
                TempData["Success"] = "Şifreniz başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Şifre değiştirilemedi. Lütfen tekrar deneyiniz.");
                return View(model);
            }
        }


    }
}
