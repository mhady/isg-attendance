using Volo.Abp.Modularity;

namespace ISG.attendance;

[DependsOn(
    typeof(attendanceApplicationModule),
    typeof(attendanceDomainTestModule)
)]
public class attendanceApplicationTestModule : AbpModule
{

}
