import { Component, OnInit } from '@angular/core';
import { ReportsService } from '../../proxy/attendance/reports.service';
import { LocationSummaryDto } from '../../proxy/attendance/models';

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
        this.summary = data || [];
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}
