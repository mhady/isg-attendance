using ISG.attendance.Samples;
using Xunit;

namespace ISG.attendance.EntityFrameworkCore.Applications;

[Collection(attendanceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<attendanceEntityFrameworkCoreTestModule>
{

}
