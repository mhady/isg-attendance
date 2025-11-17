using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISG.attendance.DTOs.Employees;
using ISG.attendance.Services;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace ISG.attendance.Controllers
{
    [RemoteService(Name = "attendance")]
    [Area("attendance")]
    [Route("api/attendance/employees")]
    public class EmployeeController : AbpController
    {
        private readonly IEmployeeAppService _employeeAppService;

        public EmployeeController(IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<EmployeeDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _employeeAppService.GetListAsync(input);
        }

        [HttpGet("{id}")]
        public virtual Task<EmployeeDto> GetAsync(Guid id)
        {
            return _employeeAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<EmployeeDto> CreateAsync(CreateEmployeeDto input)
        {
            return _employeeAppService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        public virtual Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto input)
        {
            return _employeeAppService.UpdateAsync(id, input);
        }

        [HttpDelete("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _employeeAppService.DeleteAsync(id);
        }

        [HttpPost("import")]
        public virtual Task<List<EmployeeDto>> ImportFromExcelAsync(List<ImportEmployeeDto> employees)
        {
            return _employeeAppService.ImportFromExcelAsync(employees);
        }

        [HttpGet("by-user/{userId}")]
        public virtual Task<EmployeeDto> GetByUserIdAsync(Guid userId)
        {
            return _employeeAppService.GetByUserIdAsync(userId);
        }

        [HttpGet("by-location/{locationId}")]
        public virtual Task<List<EmployeeDto>> GetEmployeesByLocationAsync(Guid locationId)
        {
            return _employeeAppService.GetEmployeesByLocationAsync(locationId);
        }
    }
}
