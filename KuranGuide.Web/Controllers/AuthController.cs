using KuranGuide.Application.DTOs.Auth;
using KuranGuide.Web.Models;
using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace KuranGuide.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiClient _api;

        public AuthController(ApiClient api)
        {
            _api = api;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // DOĞRU KULLANIM → TRequest = LoginViewModel, TResponse = LoginDto
            var response = await _api.PostAsync<LoginViewModel, LoginResponseDto>("api/auth/login", model);

            if (response == null || string.IsNullOrWhiteSpace(response.Token))
            {
                TempData["Error"] = "Giriş yapılamadı. E-posta veya şifre hatalı.";
                return View(model);
            }

            Response.Cookies.Append("jwt", response.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,                     // HTTPS şart
                SameSite = SameSiteMode.None,      // API'ye gitmesi için ZORUNLU Strict
                Expires = DateTime.UtcNow.AddDays(7),
                Path = "/"
            });


            // Kullanıcının emailini normal cookie'de tutabiliriz (opsiyonel)
            Response.Cookies.Append("email", model.Email, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7)
            });
            Response.Cookies.Append("userId", response.UserId.ToString(), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7)
            });
            // 4. (YENİ) Ad Soyad Cookie - Frontend'de "Hoşgeldin Ahmet" demek için
            // Türkçe karakter sorunu olmaması için UrlEncode edilebilir ama basit tutalım şimdilik.
            if (!string.IsNullOrEmpty(response.Ad) && !string.IsNullOrEmpty(response.Soyad))
            {
                Response.Cookies.Append("fullname", (response.Ad + " " + response.Soyad), new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(7)
                });
            }
            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // DOĞRU KULLANIM → TRequest = RegisterViewModel, TResponse = bool
            var response = await _api.PostAsync<RegisterViewModel, RegisterResponseViewModel>("api/auth/register", model);

            if (response == null || response.Success == false)
            {
                TempData["Error"] = "Kayıt oluşturulamadı. Lütfen tekrar deneyiniz.";
                return View(model);
            }
            TempData["Success"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("email");
            Response.Cookies.Delete("userId");
            Response.Cookies.Delete("fullname");
            return RedirectToAction("Index", "Home");
        }


        public IActionResult YetkisizErisim()
        {
            return View();
        }


    }
}
