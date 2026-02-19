using Microsoft.AspNetCore.Mvc;
using KuranGuide.Application.Interfaces.Services;

namespace KuranGuide.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SureController : ControllerBase
    {
        private readonly ISureService _sureService;

        public SureController(ISureService sureService)
        {
            _sureService = sureService;
        }

        // GET: api/sure
        // Tüm sure listesi (Fatiha, Bakara...)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _sureService.GetAllAsync();
            return Ok(list);
        }

        // GET: api/sure/1
        // Fatiha suresi ve ayetleri
        [HttpGet("{sureNo:int}")]
        public async Task<IActionResult> GetDetail(int sureNo)
        {
            var sure = await _sureService.GetSureDetailAsync(sureNo);
            if (sure == null) return NotFound("Sure bulunamadı.");
            return Ok(sure);
        }


    }
}
