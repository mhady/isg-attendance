using System.Threading.Tasks;

namespace ISG.attendance.Data;

public interface IattendanceDbSchemaMigrator
{
    Task MigrateAsync();
}
