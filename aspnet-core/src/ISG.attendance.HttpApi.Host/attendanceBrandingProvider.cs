using Microsoft.Extensions.Localization;
using ISG.attendance.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace ISG.attendance;

[Dependency(ReplaceServices = true)]
public class attendanceBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<attendanceResource> _localizer;

    public attendanceBrandingProvider(IStringLocalizer<attendanceResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
