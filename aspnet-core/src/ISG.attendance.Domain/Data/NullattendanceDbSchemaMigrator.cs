using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ISG.attendance.Data;

/* This is used if database provider does't define
 * IattendanceDbSchemaMigrator implementation.
 */
public class NullattendanceDbSchemaMigrator : IattendanceDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
