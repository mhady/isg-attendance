using System;
using System.Threading.Tasks;
using ISG.attendance.DTOs.Settings;
using ISG.attendance.Entities;
using ISG.attendance.Permissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ISG.attendance.Services
{
    public class CompanySettingsAppService : ApplicationService, ICompanySettingsAppService
    {
        private readonly IRepository<CompanySettings, Guid> _settingsRepository;

        public CompanySettingsAppService(IRepository<CompanySettings, Guid> settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<CompanySettingsDto> GetAsync()
        {
            await CheckPolicyAsync(attendancePermissions.Settings.Default);

            var settings = await _settingsRepository.FirstOrDefaultAsync();

            if (settings == null)
            {
                // Return default settings
                return new CompanySettingsDto
                {
                    NormalWorkingHours = 8.0,
                    WorkdayStartTime = new TimeSpan(8, 0, 0),
                    WorkdayEndTime = new TimeSpan(17, 0, 0),
                    LateCheckInGracePeriodMinutes = 0,
                    EarlyCheckOutGracePeriodMinutes = 0,
                    TimeZone = "UTC"
                };
            }

            return ObjectMapper.Map<CompanySettings, CompanySettingsDto>(settings);
        }

        public async Task<CompanySettingsDto> CreateOrUpdateAsync(CreateUpdateCompanySettingsDto input)
        {
            await CheckPolicyAsync(attendancePermissions.Settings.Manage);

            var settings = await _settingsRepository.FirstOrDefaultAsync();

            if (settings == null)
            {
                // Create new settings
                settings = new CompanySettings(
                    GuidGenerator.Create(),
                    input.NormalWorkingHours,
                    input.WorkdayStartTime,
                    input.WorkdayEndTime,
                    CurrentTenant.Id
                );

                settings.UpdateSettings(
                    input.NormalWorkingHours,
                    input.WorkdayStartTime,
                    input.WorkdayEndTime,
                    input.LateCheckInGracePeriodMinutes,
                    input.EarlyCheckOutGracePeriodMinutes,
                    input.TimeZone
                );

                await _settingsRepository.InsertAsync(settings);
            }
            else
            {
                // Update existing settings
                settings.UpdateSettings(
                    input.NormalWorkingHours,
                    input.WorkdayStartTime,
                    input.WorkdayEndTime,
                    input.LateCheckInGracePeriodMinutes,
                    input.EarlyCheckOutGracePeriodMinutes,
                    input.TimeZone
                );

                await _settingsRepository.UpdateAsync(settings);
            }

            return ObjectMapper.Map<CompanySettings, CompanySettingsDto>(settings);
        }

        private async Task CheckPolicyAsync(string policyName)
        {
            await AuthorizationService.CheckAsync(policyName);
        }
    }
}
