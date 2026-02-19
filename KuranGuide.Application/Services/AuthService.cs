using KuranGuide.Application.DTOs.Auth;
using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KuranGuide.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IKullaniciRepository _kullaniciRepo;
        private readonly IConfiguration _config;

        public AuthService(IKullaniciRepository kullaniciRepo, IConfiguration config)
        {
            _kullaniciRepo = kullaniciRepo;
            _config = config;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            var existing = await _kullaniciRepo.FindAsync(x => x.Email == dto.Email);
            if (existing.Any())
                return false;

            var user = new Kullanici
            {
                Ad = dto.Ad,      
                Soyad = dto.Soyad,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                Role = "User",
                CreatedAt = DateTime.Now
            };

            await _kullaniciRepo.AddAsync(user);
            await _kullaniciRepo.SaveChangesAsync();

            return true;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var allUsers = await _kullaniciRepo.GetAllAsync();
            var cleanEmail = dto.Email.Trim();
            var users = await _kullaniciRepo.FindAsync(x => x.Email.Trim().ToLower() == cleanEmail);
            var user = users.FirstOrDefault();
            if (user == null)
                return null;

            if (user.PasswordHash != HashPassword(dto.Password))
                return null;

            return new LoginResponseDto
            {
                Token = GenerateToken(user),
                Email = user.Email,
                UserId = user.Id,
                Ad = user.Ad,
                Soyad = user.Soyad
            };
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private string GenerateToken(Kullanici user)
        {
            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Ad)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(int.Parse(_config["Jwt:ExpiresHours"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
