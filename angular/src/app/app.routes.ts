import { Routes } from '@angular/router';

export const appRoutes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () => import('./home/home.routes').then(m => m.homeRoutes),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.createRoutes()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(m => m.createRoutes()),
  },
  {
    path: 'tenant-management',
    loadChildren: () =>
      import('@abp/ng.tenant-management').then(m => m.createRoutes()),
  },
  {
    path: 'setting-management',
    loadChildren: () =>
      import('@abp/ng.setting-management').then(m => m.createRoutes()),
  },
  {
    path: 'attendance/employees',
    loadChildren: () =>
      import('./attendance/employees/employees.module').then(m => m.EmployeesModule),
  },
  {
    path: 'attendance/locations',
    loadChildren: () =>
      import('./attendance/locations/locations.module').then(m => m.LocationsModule),
  },
  {
    path: 'attendance/attendances',
    loadChildren: () =>
      import('./attendance/attendances/attendances.module').then(m => m.AttendancesModule),
  },
  {
    path: 'attendance/settings',
    loadChildren: () =>
      import('./attendance/settings/settings.module').then(m => m.SettingsModule),
  },
  {
    path: 'attendance/reports',
    loadChildren: () =>
      import('./attendance/reports/reports.module').then(m => m.ReportsModule),
  },
];
