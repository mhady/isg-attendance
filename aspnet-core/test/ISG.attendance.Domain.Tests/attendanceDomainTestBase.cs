using Volo.Abp.Modularity;

namespace ISG.attendance;

/* Inherit from this class for your domain layer tests. */
public abstract class attendanceDomainTestBase<TStartupModule> : attendanceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
