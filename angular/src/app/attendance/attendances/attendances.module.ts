import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../../shared/shared.module';
import { AttendancesRoutingModule } from './attendances-routing.module';
import { AttendanceListComponent } from './attendance-list.component';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbModule,
    SharedModule,
    AttendancesRoutingModule,
    AttendanceListComponent,
  ],
})
export class AttendancesModule {}
