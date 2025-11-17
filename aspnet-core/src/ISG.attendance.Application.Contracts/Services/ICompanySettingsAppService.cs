using System.Threading.Tasks;
using ISG.attendance.DTOs.Settings;
using Volo.Abp.Application.Services;

namespace ISG.attendance.Services
{
    public interface ICompanySettingsAppService : IApplicationService
    {
        Task<CompanySettingsDto> GetAsync();

        Task<CompanySettingsDto> CreateOrUpdateAsync(CreateUpdateCompanySettingsDto input);
    }
}
