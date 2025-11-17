# Angular Frontend Implementation Guide

This guide provides step-by-step instructions for implementing the Angular frontend components for the attendance management system.

## Prerequisites

- Angular CLI installed
- Basic knowledge of Angular and TypeScript
- ABP Angular UI understanding

## Module Structure

Create the following modules in your Angular application:

```
src/app/
├── attendance/
│   ├── employees/
│   │   ├── employee-list/
│   │   ├── employee-form/
│   │   └── employee-import/
│   ├── locations/
│   │   ├── location-list/
│   │   └── location-form/
│   ├── attendances/
│   │   ├── attendance-list/
│   │   └── attendance-view/
│   ├── settings/
│   │   └── company-settings/
│   └── reports/
│       ├── location-summary/
│       └── monthly-report/
└── proxy/
    └── attendance/     # Already created
```

## 1. Employee Management Module

### Generate Components

```bash
cd angular/src/app
ng generate module attendance/employees --routing
ng generate component attendance/employees/employee-list
ng generate component attendance/employees/employee-form
ng generate component attendance/employees/employee-import
```

### Employee List Component

**employee-list.component.ts:**

```typescript
import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../../../proxy/attendance/employee.service';
import { LocationService } from '../../../proxy/attendance/location.service';
import { EmployeeDto, LocationDto } from '../../../proxy/attendance/models';
import { PagedAndSortedResultRequestDto } from '@abp/ng.core';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
})
export class EmployeeListComponent implements OnInit {
  employees: EmployeeDto[] = [];
  locations: LocationDto[] = [];
  totalCount = 0;
  loading = false;

  pageQuery: PagedAndSortedResultRequestDto = {
    skipCount: 0,
    maxResultCount: 10,
    sorting: ''
  };

  constructor(
    private employeeService: EmployeeService,
    private locationService: LocationService
  ) {}

  ngOnInit() {
    this.loadEmployees();
    this.loadLocations();
  }

  loadEmployees() {
    this.loading = true;
    this.employeeService.getList(this.pageQuery).subscribe({
      next: (response) => {
        this.employees = response.items;
        this.totalCount = response.totalCount;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  loadLocations() {
    this.locationService.getAllLocations().subscribe({
      next: (response) => {
        this.locations = response.items;
      }
    });
  }

  onPageChange(page: number) {
    this.pageQuery.skipCount = (page - 1) * this.pageQuery.maxResultCount;
    this.loadEmployees();
  }

  createEmployee() {
    // Open create modal or navigate to form
  }

  editEmployee(id: string) {
    // Open edit modal or navigate to form
  }

  deleteEmployee(id: string) {
    if (confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.delete(id).subscribe(() => {
        this.loadEmployees();
      });
    }
  }

  importFromExcel() {
    // Open import modal
  }
}
```

**employee-list.component.html:**

```html
<div class="card">
  <div class="card-header">
    <div class="row">
      <div class="col">
        <h3>Employees</h3>
      </div>
      <div class="col text-end">
        <button class="btn btn-primary me-2" (click)="createEmployee()">
          <i class="fa fa-plus"></i> Add Employee
        </button>
        <button class="btn btn-success" (click)="importFromExcel()">
          <i class="fa fa-file-excel"></i> Import from Excel
        </button>
      </div>
    </div>
  </div>

  <div class="card-body">
    <div class="table-responsive">
      <table class="table table-striped">
        <thead>
          <tr>
            <th>Employee Code</th>
            <th>Full Name</th>
            <th>Email</th>
            <th>Phone Number</th>
            <th>Location</th>
            <th>Status</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let employee of employees">
            <td>{{ employee.employeeCode }}</td>
            <td>{{ employee.fullName }}</td>
            <td>{{ employee.email }}</td>
            <td>{{ employee.phoneNumber }}</td>
            <td>{{ employee.locationName }}</td>
            <td>
              <span class="badge" [class.bg-success]="employee.isActive" [class.bg-danger]="!employee.isActive">
                {{ employee.isActive ? 'Active' : 'Inactive' }}
              </span>
            </td>
            <td>
              <button class="btn btn-sm btn-primary me-1" (click)="editEmployee(employee.id)">
                <i class="fa fa-edit"></i>
              </button>
              <button class="btn btn-sm btn-danger" (click)="deleteEmployee(employee.id)">
                <i class="fa fa-trash"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Pagination -->
    <ngb-pagination
      [collectionSize]="totalCount"
      [(page)]="pageQuery.skipCount"
      [pageSize]="pageQuery.maxResultCount"
      (pageChange)="onPageChange($event)"
    ></ngb-pagination>
  </div>
</div>
```

### Employee Import Component

**employee-import.component.ts:**

```typescript
import { Component } from '@angular/core';
import { EmployeeService } from '../../../proxy/attendance/employee.service';
import { ImportEmployeeDto } from '../../../proxy/attendance/models';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-employee-import',
  templateUrl: './employee-import.component.html',
})
export class EmployeeImportComponent {
  employees: ImportEmployeeDto[] = [];
  importing = false;

  constructor(private employeeService: EmployeeService) {}

  onFileChange(event: any) {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onload = (e: any) => {
      const data = e.target.result;
      const workbook = XLSX.read(data, { type: 'binary' });
      const sheetName = workbook.SheetNames[0];
      const worksheet = workbook.Sheets[sheetName];
      const json = XLSX.utils.sheet_to_json(worksheet);

      this.employees = json.map((row: any) => ({
        fullName: row['FullName'],
        email: row['Email'],
        password: row['Password'] || 'TempPassword123!',
        phoneNumber: row['PhoneNumber'],
        employeeCode: row['EmployeeCode'],
        locationName: row['LocationName']
      }));
    };

    reader.readAsBinaryString(file);
  }

  downloadTemplate() {
    const template = [
      {
        FullName: 'John Doe',
        Email: 'john.doe@company.com',
        Password: 'Pass123!',
        PhoneNumber: '+1234567890',
        EmployeeCode: 'EMP001',
        LocationName: 'Main Office'
      }
    ];

    const worksheet = XLSX.utils.json_to_sheet(template);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Employees');
    XLSX.writeFile(workbook, 'employee-import-template.xlsx');
  }

  import() {
    this.importing = true;
    this.employeeService.importFromExcel(this.employees).subscribe({
      next: (result) => {
        alert(`Successfully imported ${result.length} employees`);
        this.importing = false;
        this.employees = [];
      },
      error: () => {
        this.importing = false;
      }
    });
  }
}
```

## 2. Location Management Module

### Generate Components

```bash
ng generate module attendance/locations --routing
ng generate component attendance/locations/location-list
ng generate component attendance/locations/location-form
```

### Location List Component

**location-list.component.ts:**

```typescript
import { Component, OnInit } from '@angular/core';
import { LocationService } from '../../../proxy/attendance/location.service';
import { LocationDto } from '../../../proxy/attendance/models';
import { PagedAndSortedResultRequestDto } from '@abp/ng.core';

@Component({
  selector: 'app-location-list',
  templateUrl: './location-list.component.html',
})
export class LocationListComponent implements OnInit {
  locations: LocationDto[] = [];
  totalCount = 0;
  loading = false;

  pageQuery: PagedAndSortedResultRequestDto = {
    skipCount: 0,
    maxResultCount: 10,
    sorting: ''
  };

  constructor(private locationService: LocationService) {}

  ngOnInit() {
    this.loadLocations();
  }

  loadLocations() {
    this.loading = true;
    this.locationService.getList(this.pageQuery).subscribe({
      next: (response) => {
        this.locations = response.items;
        this.totalCount = response.totalCount;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  deleteLocation(id: string) {
    if (confirm('Are you sure you want to delete this location?')) {
      this.locationService.delete(id).subscribe(() => {
        this.loadLocations();
      });
    }
  }
}
```

## 3. Attendance View Module

### Attendance List Component

```typescript
import { Component, OnInit } from '@angular/core';
import { AttendanceService } from '../../../proxy/attendance/attendance.service';
import { AttendanceDto } from '../../../proxy/attendance/models';
import { PagedAndSortedResultRequestDto } from '@abp/ng.core';

@Component({
  selector: 'app-attendance-list',
  templateUrl: './attendance-list.component.html',
})
export class AttendanceListComponent implements OnInit {
  attendances: AttendanceDto[] = [];
  totalCount = 0;
  loading = false;
  selectedDate: Date = new Date();

  pageQuery: PagedAndSortedResultRequestDto = {
    skipCount: 0,
    maxResultCount: 10,
    sorting: ''
  };

  constructor(private attendanceService: AttendanceService) {}

  ngOnInit() {
    this.loadAttendances();
  }

  loadAttendances() {
    this.loading = true;
    const dateStr = this.selectedDate.toISOString().split('T')[0];

    this.attendanceService.getAttendanceByDate(dateStr, this.pageQuery).subscribe({
      next: (response) => {
        this.attendances = response.items;
        this.totalCount = response.totalCount;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  onDateChange(date: Date) {
    this.selectedDate = date;
    this.loadAttendances();
  }

  formatHours(hours: number): string {
    const h = Math.floor(hours);
    const m = Math.floor((hours - h) * 60);
    return `${h}h ${m}m`;
  }
}
```

## 4. Settings Module

### Company Settings Component

```typescript
import { Component, OnInit } from '@angular/core';
import { SettingsService } from '../../../proxy/attendance/settings.service';
import { CompanySettingsDto, CreateUpdateCompanySettingsDto } from '../../../proxy/attendance/models';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-company-settings',
  templateUrl: './company-settings.component.html',
})
export class CompanySettingsComponent implements OnInit {
  settingsForm: FormGroup;
  loading = false;
  saving = false;

  constructor(
    private settingsService: SettingsService,
    private fb: FormBuilder
  ) {
    this.settingsForm = this.fb.group({
      normalWorkingHours: [8, [Validators.required, Validators.min(1), Validators.max(24)]],
      workdayStartTime: ['08:00', Validators.required],
      workdayEndTime: ['17:00', Validators.required],
      lateCheckInGracePeriodMinutes: [0, [Validators.min(0), Validators.max(120)]],
      earlyCheckOutGracePeriodMinutes: [0, [Validators.min(0), Validators.max(120)]],
      timeZone: ['UTC', Validators.required]
    });
  }

  ngOnInit() {
    this.loadSettings();
  }

  loadSettings() {
    this.loading = true;
    this.settingsService.get().subscribe({
      next: (settings) => {
        this.settingsForm.patchValue(settings);
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  save() {
    if (this.settingsForm.invalid) {
      return;
    }

    this.saving = true;
    const settings: CreateUpdateCompanySettingsDto = this.settingsForm.value;

    this.settingsService.createOrUpdate(settings).subscribe({
      next: () => {
        alert('Settings saved successfully');
        this.saving = false;
      },
      error: () => {
        this.saving = false;
      }
    });
  }
}
```

## 5. Reports Module

### Location Summary Component

```typescript
import { Component, OnInit } from '@angular/core';
import { ReportsService } from '../../../proxy/attendance/reports.service';
import { LocationSummaryDto } from '../../../proxy/attendance/models';

@Component({
  selector: 'app-location-summary',
  templateUrl: './location-summary.component.html',
})
export class LocationSummaryComponent implements OnInit {
  summary: LocationSummaryDto[] = [];
  loading = false;

  constructor(private reportsService: ReportsService) {}

  ngOnInit() {
    this.loadSummary();
  }

  loadSummary() {
    this.loading = true;
    this.reportsService.getLocationSummary().subscribe({
      next: (data) => {
        this.summary = data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}
```

### Monthly Attendance Report Component

```typescript
import { Component } from '@angular/core';
import { ReportsService } from '../../../proxy/attendance/reports.service';
import { MonthlyAttendanceReportDto, GetMonthlyAttendanceInput } from '../../../proxy/attendance/models';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-monthly-report',
  templateUrl: './monthly-report.component.html',
})
export class MonthlyReportComponent {
  reportForm: FormGroup;
  reports: MonthlyAttendanceReportDto[] = [];
  loading = false;

  constructor(
    private reportsService: ReportsService,
    private fb: FormBuilder
  ) {
    const now = new Date();
    this.reportForm = this.fb.group({
      month: [now.getMonth() + 1, [Validators.required, Validators.min(1), Validators.max(12)]],
      year: [now.getFullYear(), [Validators.required, Validators.min(2000)]],
      employeeName: ['']
    });
  }

  generate() {
    if (this.reportForm.invalid) {
      return;
    }

    this.loading = true;
    const input: GetMonthlyAttendanceInput = this.reportForm.value;

    this.reportsService.getMonthlyAttendanceReport(input).subscribe({
      next: (data) => {
        this.reports = data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  exportToExcel() {
    // Implement Excel export using xlsx library
  }
}
```

## 6. Routing Configuration

**attendance-routing.module.ts:**

```typescript
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PermissionGuard } from '@abp/ng.core';

const routes: Routes = [
  {
    path: 'employees',
    loadChildren: () => import('./employees/employees.module').then(m => m.EmployeesModule),
    canActivate: [PermissionGuard],
    data: { requiredPolicy: 'attendance.Employees' }
  },
  {
    path: 'locations',
    loadChildren: () => import('./locations/locations.module').then(m => m.LocationsModule),
    canActivate: [PermissionGuard],
    data: { requiredPolicy: 'attendance.Locations' }
  },
  {
    path: 'attendances',
    loadChildren: () => import('./attendances/attendances.module').then(m => m.AttendancesModule),
    canActivate: [PermissionGuard],
    data: { requiredPolicy: 'attendance.Attendances' }
  },
  {
    path: 'settings',
    loadChildren: () => import('./settings/settings.module').then(m => m.SettingsModule),
    canActivate: [PermissionGuard],
    data: { requiredPolicy: 'attendance.Settings' }
  },
  {
    path: 'reports',
    loadChildren: () => import('./reports/reports.module').then(m => m.ReportsModule),
    canActivate: [PermissionGuard],
    data: { requiredPolicy: 'attendance.Reports' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AttendanceRoutingModule { }
```

## 7. Navigation Menu

Add to `route.provider.ts`:

```typescript
{
  path: '/attendance',
  name: '::Menu:Attendance',
  iconClass: 'fas fa-clock',
  order: 2,
  layout: eLayoutType.application,
  requiredPolicy: 'attendance.Employees || attendance.Locations || attendance.Attendances',
},
{
  path: '/attendance/employees',
  name: '::Menu:Employees',
  parentName: '::Menu:Attendance',
  requiredPolicy: 'attendance.Employees',
},
{
  path: '/attendance/locations',
  name: '::Menu:Locations',
  parentName: '::Menu:Attendance',
  requiredPolicy: 'attendance.Locations',
},
{
  path: '/attendance/attendances',
  name: '::Menu:Attendances',
  parentName: '::Menu:Attendance',
  requiredPolicy: 'attendance.Attendances',
},
{
  path: '/attendance/reports',
  name: '::Menu:Reports',
  parentName: '::Menu:Attendance',
  requiredPolicy: 'attendance.Reports',
},
{
  path: '/attendance/settings',
  name: '::Menu:Settings',
  parentName: '::Menu:Attendance',
  requiredPolicy: 'attendance.Settings',
},
```

## 8. Required Packages

Make sure to install required packages:

```bash
yarn add @ng-bootstrap/ng-bootstrap
yarn add xlsx
yarn add @types/xlsx --dev
```

## Best Practices

1. **Error Handling**: Always handle errors in subscriptions
2. **Loading States**: Show loading indicators during API calls
3. **Confirmation Dialogs**: Use confirmations for delete operations
4. **Form Validation**: Validate all form inputs
5. **Permissions**: Use permission guards on routes
6. **Responsive Design**: Ensure UI works on all screen sizes
7. **Code Reusability**: Create shared components for common patterns

## Testing

Create unit tests for components:

```typescript
describe('EmployeeListComponent', () => {
  let component: EmployeeListComponent;
  let fixture: ComponentFixture<EmployeeListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmployeeListComponent ],
      imports: [ HttpClientTestingModule ]
    })
    .compileComponents();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load employees on init', () => {
    component.ngOnInit();
    expect(component.loading).toBe(true);
  });
});
```

## Next Steps

1. Generate all components using Angular CLI
2. Implement the HTML templates
3. Add styling with Bootstrap classes
4. Test each module independently
5. Integrate with the main application
6. Add localization support
7. Implement advanced features (filters, exports, etc.)
