using ISG.attendance.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ISG.attendance.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(attendanceEntityFrameworkCoreModule),
    typeof(attendanceApplicationContractsModule)
    )]
public class attendanceDbMigratorModule : AbpModule
{
}
