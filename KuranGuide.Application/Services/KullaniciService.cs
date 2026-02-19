using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Application.Interfaces.Services;
using KuranGuide.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace KuranGuide.Application.Services
{
    public class KullaniciService : IKullaniciService
    {
        private readonly IKullaniciRepository _repo;

        public KullaniciService(IKullaniciRepository repo)
        {
            _repo = repo;
        }
        public async Task<Kullanici> CreateAsync(Kullanici user)
        {
            var userHash = new Kullanici
            {
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Role = user.Role,
                PasswordHash = HashPassword(user.PasswordHash)
            };
            await _repo.AddAsync(userHash);
            await _repo.SaveChangesAsync();
            return userHash;
        }

        public async Task<bool> UpdatePasswordAsync(int userId, string newPassword)
        {
            var user = await _repo.GetByIdAsync(userId);
            if (user == null) return false;
            user.PasswordHash = HashPassword(newPassword);
            await _repo.UpdateAsync(user);
            await _repo.SaveChangesAsync();

            return true;
        }
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<Kullanici> UpdateAsync(Kullanici user)
        {
            await _repo.UpdateAsync(user);
            await _repo.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(entity);
            await _repo.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Kullanici>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Kullanici> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Kullanici> GetByEmailAsync(string email)
        {
            var users = await _repo.FindAsync(u => u.Email == email);
            return users.FirstOrDefault();
        }

        public async Task<Kullanici> RegisterAsync(Kullanici user)
        {
            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var users = await _repo.FindAsync(u => u.Email == email);
            return users.Any();
        }
    }
}
