import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { EmployeeService } from '../../proxy/attendance/employee.service';
import { LocationService } from '../../proxy/attendance/location.service';
import { EmployeeDto, LocationDto, CreateEmployeeDto, UpdateEmployeeDto } from '../../proxy/attendance/models';
import { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { SharedModule } from '../../shared/shared.module';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NgbModule, SharedModule],
})
export class EmployeeListComponent implements OnInit {
  employees: EmployeeDto[] = [];
  locations: LocationDto[] = [];
  totalCount = 0;
  loading = false;
  isModalOpen = false;
  selectedEmployee: EmployeeDto | null = null;

  employeeForm: FormGroup;

  pageQuery: PagedAndSortedResultRequestDto = {
    skipCount: 0,
    maxResultCount: 10,
    sorting: ''
  };

  constructor(
    private employeeService: EmployeeService,
    private locationService: LocationService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {
    this.employeeForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.maxLength(256)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
      password: ['', [Validators.maxLength(256)]],
      phoneNumber: ['', [Validators.maxLength(50)]],
      employeeCode: ['', [Validators.maxLength(50)]],
      locationId: [null],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.loadEmployees();
    this.loadLocations();
  }

  loadEmployees() {
    this.loading = true;
    this.employeeService.getList(this.pageQuery).subscribe({
      next: (response) => {
        this.employees = response.items || [];
        this.totalCount = response.totalCount || 0;
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
        this.locations = response.items || [];
      }
    });
  }

  onPageChange(page: number) {
    this.pageQuery.skipCount = (page - 1) * this.pageQuery.maxResultCount;
    this.loadEmployees();
  }

  createEmployee() {
    this.selectedEmployee = null;
    this.employeeForm.reset({ isActive: true });
    this.employeeForm.get('password')?.setValidators([Validators.required, Validators.maxLength(256)]);
    this.employeeForm.get('password')?.updateValueAndValidity();
    this.isModalOpen = true;
  }

  editEmployee(employee: EmployeeDto) {
    this.selectedEmployee = employee;
    this.employeeForm.patchValue({
      fullName: employee.fullName,
      email: employee.email,
      phoneNumber: employee.phoneNumber,
      employeeCode: employee.employeeCode,
      locationId: employee.locationId,
      isActive: employee.isActive
    });
    this.employeeForm.get('password')?.clearValidators();
    this.employeeForm.get('password')?.updateValueAndValidity();
    this.isModalOpen = true;
  }

  save() {
    if (this.employeeForm.invalid) {
      return;
    }

    if (this.selectedEmployee) {
      // Update
      const updateDto: UpdateEmployeeDto = {
        fullName: this.employeeForm.value.fullName,
        email: this.employeeForm.value.email,
        phoneNumber: this.employeeForm.value.phoneNumber,
        employeeCode: this.employeeForm.value.employeeCode,
        locationId: this.employeeForm.value.locationId,
        isActive: this.employeeForm.value.isActive
      };

      this.employeeService.update(this.selectedEmployee.id, updateDto).subscribe(() => {
        this.isModalOpen = false;
        this.loadEmployees();
      });
    } else {
      // Create
      const createDto: CreateEmployeeDto = this.employeeForm.value;

      this.employeeService.create(createDto).subscribe(() => {
        this.isModalOpen = false;
        this.loadEmployees();
      });
    }
  }

  deleteEmployee(id: string) {
    this.confirmation.warn('::AreYouSure', '::AreYouSureToDelete').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.employeeService.delete(id).subscribe(() => {
          this.loadEmployees();
        });
      }
    });
  }
}
