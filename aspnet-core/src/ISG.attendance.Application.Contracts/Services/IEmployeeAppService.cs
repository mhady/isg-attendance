using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISG.attendance.DTOs.Employees;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ISG.attendance.Services
{
    public interface IEmployeeAppService : IApplicationService
    {
        Task<PagedResultDto<EmployeeDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<EmployeeDto> GetAsync(Guid id);

        Task<EmployeeDto> CreateAsync(CreateEmployeeDto input);

        Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto input);

        Task DeleteAsync(Guid id);

        Task<List<EmployeeDto>> ImportFromExcelAsync(List<ImportEmployeeDto> employees);

        Task<EmployeeDto> GetByUserIdAsync(Guid userId);

        Task<List<EmployeeDto>> GetEmployeesByLocationAsync(Guid locationId);
    }
}
