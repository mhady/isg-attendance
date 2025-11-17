import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto, ListResultDto } from '@abp/ng.core';
import type { LocationDto, CreateUpdateLocationDto } from './models';

@Injectable({
  providedIn: 'root',
})
export class LocationService {
  apiName = 'attendance';

  constructor(private restService: RestService) {}

  getList = (input: PagedAndSortedResultRequestDto) =>
    this.restService.request<any, PagedResultDto<LocationDto>>({
      method: 'GET',
      url: '/api/attendance/locations',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, LocationDto>({
      method: 'GET',
      url: `/api/attendance/locations/${id}`,
    },
    { apiName: this.apiName });

  create = (input: CreateUpdateLocationDto) =>
    this.restService.request<any, LocationDto>({
      method: 'POST',
      url: '/api/attendance/locations',
      body: input,
    },
    { apiName: this.apiName });

  update = (id: string, input: CreateUpdateLocationDto) =>
    this.restService.request<any, LocationDto>({
      method: 'PUT',
      url: `/api/attendance/locations/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/attendance/locations/${id}`,
    },
    { apiName: this.apiName });

  getAllLocations = () =>
    this.restService.request<any, ListResultDto<LocationDto>>({
      method: 'GET',
      url: '/api/attendance/locations/all',
    },
    { apiName: this.apiName });
}
