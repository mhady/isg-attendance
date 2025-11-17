using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ISG.attendance.Data;
using Volo.Abp.DependencyInjection;

namespace ISG.attendance.EntityFrameworkCore;

public class EntityFrameworkCoreattendanceDbSchemaMigrator
    : IattendanceDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreattendanceDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the attendanceDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<attendanceDbContext>()
            .Database
            .MigrateAsync();
    }
}
