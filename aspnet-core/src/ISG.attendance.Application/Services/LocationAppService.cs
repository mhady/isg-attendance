using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISG.attendance.DTOs.Locations;
using ISG.attendance.Entities;
using ISG.attendance.Permissions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ISG.attendance.Services
{
    public class LocationAppService : CrudAppService<
        Location,
        LocationDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateLocationDto,
        CreateUpdateLocationDto>,
        ILocationAppService
    {
        private readonly IRepository<Employee, Guid> _employeeRepository;

        public LocationAppService(
            IRepository<Location, Guid> repository,
            IRepository<Employee, Guid> employeeRepository)
            : base(repository)
        {
            _employeeRepository = employeeRepository;

            GetPolicyName = attendancePermissions.Locations.Default;
            GetListPolicyName = attendancePermissions.Locations.Default;
            CreatePolicyName = attendancePermissions.Locations.Create;
            UpdatePolicyName = attendancePermissions.Locations.Edit;
            DeletePolicyName = attendancePermissions.Locations.Delete;
        }

        protected override async Task<IQueryable<Location>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            return (await base.CreateFilteredQueryAsync(input))
                .Include(x => x.CreationTime)
                .OrderByDescending(x => x.CreationTime);
        }

        protected override async Task<LocationDto> MapToGetOutputDtoAsync(Location entity)
        {
            var dto = await base.MapToGetOutputDtoAsync(entity);

            // Get employee count for this location
            dto.EmployeeCount = await _employeeRepository
                .CountAsync(e => e.LocationId == entity.Id && e.IsActive);

            return dto;
        }

        protected override async Task<List<LocationDto>> MapToGetListOutputDtosAsync(List<Location> entities)
        {
            var dtos = await base.MapToGetListOutputDtosAsync(entities);

            // Get employee counts for all locations
            var locationIds = entities.Select(e => e.Id).ToList();
            var employeeCounts = await _employeeRepository
                .Where(e => locationIds.Contains(e.LocationId.Value) && e.IsActive)
                .GroupBy(e => e.LocationId)
                .Select(g => new { LocationId = g.Key, Count = g.Count() })
                .ToListAsync();

            foreach (var dto in dtos)
            {
                var count = employeeCounts.FirstOrDefault(ec => ec.LocationId == dto.Id);
                dto.EmployeeCount = count?.Count ?? 0;
            }

            return dtos;
        }

        public async Task<ListResultDto<LocationDto>> GetAllLocationsAsync()
        {
            var locations = await Repository.GetListAsync();
            var dtos = await MapToGetListOutputDtosAsync(locations);
            return new ListResultDto<LocationDto>(dtos);
        }
    }
}
