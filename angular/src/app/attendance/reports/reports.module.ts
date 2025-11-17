import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../../shared/shared.module';
import { ReportsRoutingModule } from './reports-routing.module';
import { LocationSummaryComponent } from './location-summary.component';
import { MonthlyReportComponent } from './monthly-report.component';

@NgModule({
  declarations: [
    LocationSummaryComponent,
    MonthlyReportComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbModule,
    SharedModule,
    ReportsRoutingModule,
  ],
})
export class ReportsModule {}
