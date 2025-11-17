using Volo.Abp.Settings;

namespace ISG.attendance.Settings;

public class attendanceSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(attendanceSettings.MySetting1));
    }
}
