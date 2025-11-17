import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import type { LocationSummaryDto, MonthlyAttendanceReportDto, GetMonthlyAttendanceInput } from './models';

@Injectable({
  providedIn: 'root',
})
export class ReportsService {
  apiName = 'attendance';

  constructor(private restService: RestService) {}

  getLocationSummary = () =>
    this.restService.request<any, LocationSummaryDto[]>({
      method: 'GET',
      url: '/api/attendance/reports/location-summary',
    },
    { apiName: this.apiName });

  getMonthlyAttendanceReport = (input: GetMonthlyAttendanceInput) =>
    this.restService.request<any, MonthlyAttendanceReportDto[]>({
      method: 'POST',
      url: '/api/attendance/reports/monthly-attendance',
      body: input,
    },
    { apiName: this.apiName });
}
