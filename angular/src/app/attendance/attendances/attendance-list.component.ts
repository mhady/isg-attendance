import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AttendanceService } from '../../proxy/attendance/attendance.service';
import { AttendanceDto } from '../../proxy/attendance/models';
import { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import { SharedModule } from '../../shared/shared.module';

@Component({
  selector: 'app-attendance-list',
  templateUrl: './attendance-list.component.html',
  standalone: true,
  imports: [CommonModule, NgbModule, SharedModule],
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
        this.attendances = response.items || [];
        this.totalCount = response.totalCount || 0;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  onDateChange(event: any) {
    this.selectedDate = new Date(event.target.value);
    this.loadAttendances();
  }

  onPageChange(page: number) {
    this.pageQuery.skipCount = (page - 1) * this.pageQuery.maxResultCount;
    this.loadAttendances();
  }

  formatHours(hours: number): string {
    const h = Math.floor(hours);
    const m = Math.floor((hours - h) * 60);
    return `${h}h ${m}m`;
  }

  formatTime(dateTime: string | undefined): string {
    if (!dateTime) return '-';
    return new Date(dateTime).toLocaleTimeString();
  }
}
