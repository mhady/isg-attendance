import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../../shared/shared.module';
import { LocationsRoutingModule } from './locations-routing.module';
import { LocationListComponent } from './location-list.component';

@NgModule({
  declarations: [
    LocationListComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbModule,
    SharedModule,
    LocationsRoutingModule,
  ],
})
export class LocationsModule {}
