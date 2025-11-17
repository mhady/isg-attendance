import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../../shared/shared.module';
import { SettingsRoutingModule } from './settings-routing.module';
import { CompanySettingsComponent } from './company-settings.component';

@NgModule({
  declarations: [
    CompanySettingsComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbModule,
    SharedModule,
    SettingsRoutingModule,
  ],
})
export class SettingsModule {}
