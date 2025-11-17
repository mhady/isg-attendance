using System;
using System.Collections.Generic;
using System.Text;
using ISG.attendance.Localization;
using Volo.Abp.Application.Services;

namespace ISG.attendance;

/* Inherit your application services from this class.
 */
public abstract class attendanceAppService : ApplicationService
{
    protected attendanceAppService()
    {
        LocalizationResource = typeof(attendanceResource);
    }
}
