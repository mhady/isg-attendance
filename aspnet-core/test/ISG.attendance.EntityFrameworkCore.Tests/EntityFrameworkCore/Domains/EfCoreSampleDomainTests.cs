using ISG.attendance.Samples;
using Xunit;

namespace ISG.attendance.EntityFrameworkCore.Domains;

[Collection(attendanceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<attendanceEntityFrameworkCoreTestModule>
{

}
