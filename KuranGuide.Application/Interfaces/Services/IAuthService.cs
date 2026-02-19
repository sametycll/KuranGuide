using KuranGuide.Application.DTOs.Auth;

namespace KuranGuide.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task<bool> RegisterAsync(RegisterDto dto);

    }
}
