using System;
using System.Threading.Tasks;
using ISG.attendance.DTOs.Locations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ISG.attendance.Services
{
    public interface ILocationAppService : ICrudAppService<
        LocationDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateLocationDto,
        CreateUpdateLocationDto>
    {
        Task<ListResultDto<LocationDto>> GetAllLocationsAsync();
    }
}
