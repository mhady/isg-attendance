using AutoMapper;
using ISG.attendance.DTOs.Attendances;
using ISG.attendance.DTOs.Employees;
using ISG.attendance.DTOs.Locations;
using ISG.attendance.DTOs.Settings;
using ISG.attendance.Entities;

namespace ISG.attendance;

public class attendanceApplicationAutoMapperProfile : Profile
{
    public attendanceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        // Location mappings
        CreateMap<Location, LocationDto>();
        CreateMap<CreateUpdateLocationDto, Location>();

        // Employee mappings
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : null));

        // Attendance mappings
        CreateMap<Attendance, AttendanceDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
            .ForMember(dest => dest.EmployeeCode, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.EmployeeCode : null));

        // Settings mappings
        CreateMap<CompanySettings, CompanySettingsDto>();
        CreateMap<CreateUpdateCompanySettingsDto, CompanySettings>();
    }
}
