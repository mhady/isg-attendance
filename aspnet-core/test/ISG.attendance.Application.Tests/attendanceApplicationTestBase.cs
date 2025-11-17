using Volo.Abp.Modularity;

namespace ISG.attendance;

public abstract class attendanceApplicationTestBase<TStartupModule> : attendanceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
