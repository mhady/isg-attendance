import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LocationService } from '../../proxy/attendance/location.service';
import { LocationDto, CreateUpdateLocationDto } from '../../proxy/attendance/models';
import { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { SharedModule } from '../../shared/shared.module';

@Component({
  selector: 'app-location-list',
  templateUrl: './location-list.component.html',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NgbModule, SharedModule],
})
export class LocationListComponent implements OnInit {
  locations: LocationDto[] = [];
  totalCount = 0;
  loading = false;
  isModalOpen = false;
  selectedLocation: LocationDto | null = null;
  currentPage = 1;

  locationForm: FormGroup;

  pageQuery: PagedAndSortedResultRequestDto = {
    skipCount: 0,
    maxResultCount: 10,
    sorting: ''
  };

  constructor(
    private locationService: LocationService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {
    this.locationForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(256)]],
      description: ['', [Validators.maxLength(1024)]],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.loadLocations();
  }

  loadLocations() {
    this.loading = true;
    this.locationService.getList(this.pageQuery).subscribe({
      next: (response) => {
        this.locations = response.items || [];
        this.totalCount = response.totalCount || 0;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  onPageChange(page: number) {
    this.pageQuery.skipCount = (page - 1) * this.pageQuery.maxResultCount;
    this.loadLocations();
  }

  createLocation() {
    this.selectedLocation = null;
    this.locationForm.reset({ isActive: true });
    this.isModalOpen = true;
  }

  editLocation(location: LocationDto) {
    this.selectedLocation = location;
    this.locationForm.patchValue(location);
    this.isModalOpen = true;
  }

  save() {
    if (this.locationForm.invalid) {
      return;
    }

    const dto: CreateUpdateLocationDto = this.locationForm.value;

    if (this.selectedLocation) {
      this.locationService.update(this.selectedLocation.id, dto).subscribe(() => {
        this.isModalOpen = false;
        this.loadLocations();
      });
    } else {
      this.locationService.create(dto).subscribe(() => {
        this.isModalOpen = false;
        this.loadLocations();
      });
    }
  }

  deleteLocation(id: string) {
    this.confirmation.warn('::AreYouSure', '::AreYouSureToDelete').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.locationService.delete(id).subscribe(() => {
          this.loadLocations();
        });
      }
    });
  }
}
