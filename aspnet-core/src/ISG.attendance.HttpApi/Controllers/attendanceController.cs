using ISG.attendance.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ISG.attendance.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class attendanceController : AbpControllerBase
{
    protected attendanceController()
    {
        LocalizationResource = typeof(attendanceResource);
    }
}
