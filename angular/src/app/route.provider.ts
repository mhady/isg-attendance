import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/attendance',
        name: '::Menu:Attendance',
        iconClass: 'fas fa-clock',
        order: 2,
        layout: eLayoutType.application,
      },
      {
        path: '/attendance/employees',
        name: '::Menu:Employees',
        parentName: '::Menu:Attendance',
        layout: eLayoutType.application,
        requiredPolicy: 'attendance.Employees',
      },
      {
        path: '/attendance/locations',
        name: '::Menu:Locations',
        parentName: '::Menu:Attendance',
        layout: eLayoutType.application,
        requiredPolicy: 'attendance.Locations',
      },
      {
        path: '/attendance/attendances',
        name: '::Menu:Attendances',
        parentName: '::Menu:Attendance',
        layout: eLayoutType.application,
        requiredPolicy: 'attendance.Attendances.View',
      },
      {
        path: '/attendance/settings',
        name: '::Menu:Settings',
        parentName: '::Menu:Attendance',
        layout: eLayoutType.application,
        requiredPolicy: 'attendance.Settings',
      },
      {
        path: '/attendance/reports/location-summary',
        name: '::Menu:LocationSummary',
        parentName: '::Menu:Attendance',
        layout: eLayoutType.application,
        requiredPolicy: 'attendance.Reports.LocationSummary',
      },
      {
        path: '/attendance/reports/monthly-attendance',
        name: '::Menu:MonthlyReport',
        parentName: '::Menu:Attendance',
        layout: eLayoutType.application,
        requiredPolicy: 'attendance.Reports.MonthlyAttendance',
      },
    ]);
  };
}
