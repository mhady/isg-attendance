import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';

import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ThemeLeptonXModule } from '@abp/ng.theme.lepton-x';
import { SideMenuLayoutModule } from '@abp/ng.theme.lepton-x/layouts';
import { AccountLayoutModule } from '@abp/ng.theme.lepton-x/account';
import { AccountConfigModule } from '@abp/ng.account/config';
import { IdentityConfigModule } from '@abp/ng.identity/config';
import { TenantManagementConfigModule } from '@abp/ng.tenant-management/config';
import { SettingManagementConfigModule } from '@abp/ng.setting-management/config';
import { FeatureManagementModule } from '@abp/ng.feature-management';

import { AppComponent } from './app.component';
import { appRoutes } from './app.routes';
import { APP_ROUTE_PROVIDER } from './route.provider';
import { environment } from '../environments/environment';
import { registerLocale } from '@abp/ng.core/locale';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(appRoutes, {
      paramsInheritanceStrategy: 'always',
    }),
    CoreModule.forRoot({
      environment,
      registerLocaleFn: registerLocale(),
    }),
    ThemeSharedModule.forRoot({
      httpErrorConfig: {
        errorScreen: {
          component: undefined,
          forWhichErrors: [401, 403, 404, 500],
          hideCloseIcon: false,
        },
      },
    }),
    AccountConfigModule.forRoot(),
    IdentityConfigModule.forRoot(),
    TenantManagementConfigModule.forRoot(),
    SettingManagementConfigModule.forRoot(),
    FeatureManagementModule.forRoot(),
    ThemeLeptonXModule.forRoot(),
    SideMenuLayoutModule.forRoot(),
    AccountLayoutModule.forRoot(),
  ],
  providers: [APP_ROUTE_PROVIDER],
  bootstrap: [AppComponent],
})
export class AppModule {}
