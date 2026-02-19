using KuranGuide.Domain.Entities;

namespace KuranGuide.Application.Interfaces.Services
{
    public interface IKullaniciService
    {
        Task<Kullanici> GetByIdAsync(int id);
        Task<Kullanici> GetByEmailAsync(string email);
        Task<Kullanici> RegisterAsync(Kullanici user);
        Task<bool> UserExistsAsync(string email);

        Task<IEnumerable<Kullanici>> GetAllAsync();


        Task<Kullanici> CreateAsync(Kullanici user);
        Task<Kullanici> UpdateAsync(Kullanici user);
        Task<bool> DeleteAsync(int id);

        Task<bool> UpdatePasswordAsync(int userId, string newPassword);
    }
}
