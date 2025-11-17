import { Component } from '@angular/core';
import { ReportsService } from '../../proxy/attendance/reports.service';
import { MonthlyAttendanceReportDto, GetMonthlyAttendanceInput } from '../../proxy/attendance/models';
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
        this.reports = data || [];
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}
