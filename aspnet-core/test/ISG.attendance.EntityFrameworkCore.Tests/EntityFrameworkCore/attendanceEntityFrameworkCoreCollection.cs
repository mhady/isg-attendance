using Xunit;

namespace ISG.attendance.EntityFrameworkCore;

[CollectionDefinition(attendanceTestConsts.CollectionDefinitionName)]
public class attendanceEntityFrameworkCoreCollection : ICollectionFixture<attendanceEntityFrameworkCoreFixture>
{

}
