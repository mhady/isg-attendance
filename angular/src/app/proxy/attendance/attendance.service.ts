import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto, ListResultDto } from '@abp/ng.core';
import type { AttendanceDto, CheckInDto, CheckOutDto } from './models';

@Injectable({
  providedIn: 'root',
})
export class AttendanceService {
  apiName = 'attendance';

  constructor(private restService: RestService) {}

  getList = (input: PagedAndSortedResultRequestDto) =>
    this.restService.request<any, PagedResultDto<AttendanceDto>>({
      method: 'GET',
      url: '/api/attendance/attendances',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, AttendanceDto>({
      method: 'GET',
      url: `/api/attendance/attendances/${id}`,
    },
    { apiName: this.apiName });

  checkIn = (input: CheckInDto) =>
    this.restService.request<any, AttendanceDto>({
      method: 'POST',
      url: '/api/attendance/attendances/check-in',
      body: input,
    },
    { apiName: this.apiName });

  checkOut = (input: CheckOutDto) =>
    this.restService.request<any, AttendanceDto>({
      method: 'POST',
      url: '/api/attendance/attendances/check-out',
      body: input,
    },
    { apiName: this.apiName });

  getTodayAttendance = () =>
    this.restService.request<any, AttendanceDto>({
      method: 'GET',
      url: '/api/attendance/attendances/today',
    },
    { apiName: this.apiName });

  getAttendanceByEmployee = (employeeId: string, input: PagedAndSortedResultRequestDto) =>
    this.restService.request<any, PagedResultDto<AttendanceDto>>({
      method: 'GET',
      url: `/api/attendance/attendances/by-employee/${employeeId}`,
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName });

  getAttendanceByDate = (date: string, input: PagedAndSortedResultRequestDto) =>
    this.restService.request<any, PagedResultDto<AttendanceDto>>({
      method: 'GET',
      url: '/api/attendance/attendances/by-date',
      params: { date, skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName });

  getAttendanceByDateRange = (startDate: string, endDate: string, employeeId?: string) =>
    this.restService.request<any, ListResultDto<AttendanceDto>>({
      method: 'GET',
      url: '/api/attendance/attendances/by-date-range',
      params: { startDate, endDate, employeeId },
    },
    { apiName: this.apiName });
}
