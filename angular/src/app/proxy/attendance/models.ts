import type { AuditedEntityDto } from '@abp/ng.core';

export interface LocationDto extends AuditedEntityDto<string> {
  name: string;
  description?: string;
  isActive: boolean;
  employeeCount: number;
}

export interface CreateUpdateLocationDto {
  name: string;
  description?: string;
  isActive: boolean;
}

export interface EmployeeDto extends AuditedEntityDto<string> {
  userId?: string;
  fullName: string;
  email: string;
  phoneNumber?: string;
  employeeCode?: string;
  locationId?: string;
  locationName?: string;
  isActive: boolean;
  locationIds: string[];
}

export interface CreateEmployeeDto {
  fullName: string;
  email: string;
  password?: string;
  phoneNumber?: string;
  employeeCode?: string;
  locationId?: string;
  isActive: boolean;
}

export interface UpdateEmployeeDto {
  fullName: string;
  email: string;
  phoneNumber?: string;
  employeeCode?: string;
  locationId?: string;
  isActive: boolean;
}

export interface ImportEmployeeDto {
  fullName: string;
  email: string;
  password?: string;
  phoneNumber?: string;
  employeeCode?: string;
  locationName?: string;
}

export interface AttendanceDto extends AuditedEntityDto<string> {
  employeeId: string;
  employeeName?: string;
  employeeCode?: string;
  date: string;
  checkInTime?: string;
  checkOutTime?: string;
  totalHours: number;
  overtimeHours: number;
  isFullDay: boolean;
  notes?: string;
}

export interface CheckInDto {
  checkInTime: string;
  notes?: string;
}

export interface CheckOutDto {
  checkOutTime: string;
  notes?: string;
}

export interface CompanySettingsDto extends AuditedEntityDto<string> {
  normalWorkingHours: number;
  workdayStartTime: string;
  workdayEndTime: string;
  lateCheckInGracePeriodMinutes: number;
  earlyCheckOutGracePeriodMinutes: number;
  timeZone: string;
}

export interface CreateUpdateCompanySettingsDto {
  normalWorkingHours: number;
  workdayStartTime: string;
  workdayEndTime: string;
  lateCheckInGracePeriodMinutes: number;
  earlyCheckOutGracePeriodMinutes: number;
  timeZone: string;
}

export interface LocationSummaryDto {
  locationId: string;
  locationName: string;
  totalEmployees: number;
  activeEmployees: number;
  presentToday: number;
}

export interface MonthlyAttendanceReportDto {
  employeeId: string;
  employeeName: string;
  employeeCode?: string;
  locationName?: string;
  year: number;
  month: number;
  totalWorkingDays: number;
  fullWorkingDays: number;
  partialWorkingDays: number;
  absentDays: number;
  totalHours: number;
  totalOvertimeHours: number;
}

export interface GetMonthlyAttendanceInput {
  employeeId?: string;
  employeeName?: string;
  month: number;
  year: number;
}

export interface EmployeeLocationDto {
  id: string;
  employeeId: string;
  locationId: string;
  locationName?: string;
  creationTime: string;
}

export interface AssignLocationsDto {
  locationIds: string[];
}
