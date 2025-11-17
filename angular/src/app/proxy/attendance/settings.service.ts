import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import type { CompanySettingsDto, CreateUpdateCompanySettingsDto } from './models';

@Injectable({
  providedIn: 'root',
})
export class SettingsService {
  apiName = 'attendance';

  constructor(private restService: RestService) {}

  get = () =>
    this.restService.request<any, CompanySettingsDto>({
      method: 'GET',
      url: '/api/attendance/settings',
    },
    { apiName: this.apiName });

  createOrUpdate = (input: CreateUpdateCompanySettingsDto) =>
    this.restService.request<any, CompanySettingsDto>({
      method: 'POST',
      url: '/api/attendance/settings',
      body: input,
    },
    { apiName: this.apiName });
}
