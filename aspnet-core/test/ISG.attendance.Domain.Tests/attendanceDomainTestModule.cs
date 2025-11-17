using Volo.Abp.Modularity;

namespace ISG.attendance;

[DependsOn(
    typeof(attendanceDomainModule),
    typeof(attendanceTestBaseModule)
)]
public class attendanceDomainTestModule : AbpModule
{

}
