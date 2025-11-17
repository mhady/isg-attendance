import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { SettingsService } from '../../proxy/attendance/settings.service';
import { CreateUpdateCompanySettingsDto } from '../../proxy/attendance/models';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToasterService } from '@abp/ng.theme.shared';
import { SharedModule } from '../../shared/shared.module';

@Component({
  selector: 'app-company-settings',
  templateUrl: './company-settings.component.html',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, SharedModule],
})
export class CompanySettingsComponent implements OnInit {
  settingsForm: FormGroup;
  loading = false;
  saving = false;

  constructor(
    private settingsService: SettingsService,
    private fb: FormBuilder,
    private toaster: ToasterService
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
        this.settingsForm.patchValue({
          normalWorkingHours: settings.normalWorkingHours,
          workdayStartTime: settings.workdayStartTime,
          workdayEndTime: settings.workdayEndTime,
          lateCheckInGracePeriodMinutes: settings.lateCheckInGracePeriodMinutes,
          earlyCheckOutGracePeriodMinutes: settings.earlyCheckOutGracePeriodMinutes,
          timeZone: settings.timeZone
        });
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
        this.toaster.success('::SettingsSavedSuccessfully');
        this.saving = false;
      },
      error: () => {
        this.saving = false;
      }
    });
  }
}
