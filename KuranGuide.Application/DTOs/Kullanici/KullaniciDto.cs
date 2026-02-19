using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranGuide.Application.DTOs.Kullanici
{
    public class KullaniciDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}
