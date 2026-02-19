using KuranGuide.Application.DTOs.Auth;
using KuranGuide.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KuranGuide.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IKullaniciService _kullaniciService;

        public AuthController(IAuthService authService, IKullaniciService kullaniciService)
        {
            _authService = authService;
            _kullaniciService = kullaniciService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // DTO üzerindeki Regex ve Required kurallarını kontrol eder
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _authService.RegisterAsync(dto);
            if (!success)
                return BadRequest("Bu email ile kayıt zaten var.");

            return Ok(new { success = true, message = "Kayıt başarılı." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null)
                return Unauthorized("Email veya şifre yanlış.");

            return Ok(result);
        }

        [HttpPost("reset-password-direct")]
        [Authorize]
        public async Task<IActionResult> ResetPasswordDirect([FromBody] ResetPasswordDirectDto dto)
        {
            // 1. Token'dan ID'yi al
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id" || c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            // 2. Servisi çağır (Tüm işi servis yapsın)
            var result = await _kullaniciService.UpdatePasswordAsync(userId, dto.NewPassword);

            if (!result)
                return NotFound("Kullanıcı bulunamadı veya işlem başarısız.");

            return Ok(new { success = true, message = "Şifre başarıyla güncellendi." });
        }


    }
}
