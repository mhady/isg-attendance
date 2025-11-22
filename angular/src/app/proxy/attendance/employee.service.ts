import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import type { EmployeeDto, CreateEmployeeDto, UpdateEmployeeDto, ImportEmployeeDto, EmployeeLocationDto, AssignLocationsDto } from './models';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  apiName = 'attendance';

  constructor(private restService: RestService) {}

  getList = (input: PagedAndSortedResultRequestDto) =>
    this.restService.request<any, PagedResultDto<EmployeeDto>>({
      method: 'GET',
      url: '/api/attendance/employees',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, EmployeeDto>({
      method: 'GET',
      url: `/api/attendance/employees/${id}`,
    },
    { apiName: this.apiName });

  create = (input: CreateEmployeeDto) =>
    this.restService.request<any, EmployeeDto>({
      method: 'POST',
      url: '/api/attendance/employees',
      body: input,
    },
    { apiName: this.apiName });

  update = (id: string, input: UpdateEmployeeDto) =>
    this.restService.request<any, EmployeeDto>({
      method: 'PUT',
      url: `/api/attendance/employees/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/attendance/employees/${id}`,
    },
    { apiName: this.apiName });

  importFromExcel = (employees: ImportEmployeeDto[]) =>
    this.restService.request<any, EmployeeDto[]>({
      method: 'POST',
      url: '/api/attendance/employees/import',
      body: employees,
    },
    { apiName: this.apiName });

  getByLocation = (locationId: string) =>
    this.restService.request<any, EmployeeDto[]>({
      method: 'GET',
      url: `/api/attendance/employees/by-location/${locationId}`,
    },
    { apiName: this.apiName });

  getEmployeeLocations = (employeeId: string) =>
    this.restService.request<any, EmployeeLocationDto[]>({
      method: 'GET',
      url: `/api/attendance/employees/${employeeId}/locations`,
    },
    { apiName: this.apiName });

  assignLocations = (employeeId: string, input: AssignLocationsDto) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/attendance/employees/${employeeId}/assign-locations`,
      body: input,
    },
    { apiName: this.apiName });

  removeLocation = (employeeId: string, locationId: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/attendance/employees/${employeeId}/locations/${locationId}`,
    },
    { apiName: this.apiName });

  getMyProfile = () =>
    this.restService.request<any, EmployeeDto>({
      method: 'GET',
      url: '/api/attendance/employees/me',
    },
    { apiName: this.apiName });
}
