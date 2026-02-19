using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]   
    public class AdminController : AdminBaseController
    {
        private readonly ApiClient _api;

        public AdminController(ApiClient api)
        {
            _api = api;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var temaCount = await _api.GetWithAuthAsync<int>("api/admin/stats/tema-count", token);
            var ayetCount = await _api.GetWithAuthAsync<int>("api/admin/stats/ayet-count", token);
            var hadisCount = await _api.GetWithAuthAsync<int>("api/admin/stats/hadis-count", token);

            var lastAyetler = await _api.GetWithAuthAsync<List<dynamic>>("api/admin/stats/latest-ayetler", token)
                               ?? new List<dynamic>();
            var lastHadisler = await _api.GetWithAuthAsync<List<dynamic>>("api/admin/stats/latest-hadisler", token)
                               ?? new List<dynamic>();

            ViewBag.TemaCount = temaCount;
            ViewBag.AyetCount = ayetCount;
            ViewBag.HadisCount = hadisCount;
            ViewBag.LastAyetler = lastAyetler;
            ViewBag.LastHadisler = lastHadisler;

            return View();
        }
    }
}
