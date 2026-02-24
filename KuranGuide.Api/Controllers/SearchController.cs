using KuranGuide.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace KuranGuide.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
                return BadRequest("Arama icin en az 2 karakter giriniz.");

            var response = await _searchService.SearchAsync(q);

            return Ok(response);
        }
    }
}