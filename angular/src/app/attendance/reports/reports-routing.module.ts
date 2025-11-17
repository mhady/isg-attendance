import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LocationSummaryComponent } from './location-summary.component';
import { MonthlyReportComponent } from './monthly-report.component';

const routes: Routes = [
  {
    path: 'location-summary',
    component: LocationSummaryComponent,
  },
  {
    path: 'monthly-attendance',
    component: MonthlyReportComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReportsRoutingModule {}
