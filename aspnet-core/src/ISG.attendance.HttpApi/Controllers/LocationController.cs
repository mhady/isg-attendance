using System;
using System.Threading.Tasks;
using ISG.attendance.DTOs.Locations;
using ISG.attendance.Services;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace ISG.attendance.Controllers
{
    [RemoteService(Name = "attendance")]
    [Area("attendance")]
    [Route("api/attendance/locations")]
    public class LocationController : AbpController
    {
        private readonly ILocationAppService _locationAppService;

        public LocationController(ILocationAppService locationAppService)
        {
            _locationAppService = locationAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<LocationDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _locationAppService.GetListAsync(input);
        }

        [HttpGet("{id}")]
        public virtual Task<LocationDto> GetAsync(Guid id)
        {
            return _locationAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<LocationDto> CreateAsync(CreateUpdateLocationDto input)
        {
            return _locationAppService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        public virtual Task<LocationDto> UpdateAsync(Guid id, CreateUpdateLocationDto input)
        {
            return _locationAppService.UpdateAsync(id, input);
        }

        [HttpDelete("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _locationAppService.DeleteAsync(id);
        }

        [HttpGet("all")]
        public virtual Task<ListResultDto<LocationDto>> GetAllLocationsAsync()
        {
            return _locationAppService.GetAllLocationsAsync();
        }
    }
}
