using System.Threading.Tasks;
using ISG.attendance.DTOs.Settings;
using ISG.attendance.Services;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace ISG.attendance.Controllers
{
    [RemoteService(Name = "attendance")]
    [Area("attendance")]
    [Route("api/attendance/settings")]
    public class CompanySettingsController : AbpController
    {
        private readonly ICompanySettingsAppService _settingsAppService;

        public CompanySettingsController(ICompanySettingsAppService settingsAppService)
        {
            _settingsAppService = settingsAppService;
        }

        [HttpGet]
        public virtual Task<CompanySettingsDto> GetAsync()
        {
            return _settingsAppService.GetAsync();
        }

        [HttpPost]
        public virtual Task<CompanySettingsDto> CreateOrUpdateAsync(CreateUpdateCompanySettingsDto input)
        {
            return _settingsAppService.CreateOrUpdateAsync(input);
        }
    }
}
