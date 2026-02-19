using KuranGuide.Application.DTOs.Sure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranGuide.Application.Interfaces.Services
{
    public interface ISureService
    {
        Task<IEnumerable<SureDto>> GetAllAsync();
        Task<SureDetailDto> GetSureDetailAsync(int sureNo);
        Task<SureDto> GetByIdAsync(int id);
    }
}
