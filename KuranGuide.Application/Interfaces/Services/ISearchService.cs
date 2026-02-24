using KuranGuide.Application.DTOs.General;

namespace KuranGuide.Application.Interfaces.Services
{
    public interface ISearchService
    {
        Task<SearchResponseDto> SearchAsync(string query);
    }
}
