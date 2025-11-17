import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'attendance',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44388/',
    redirectUri: baseUrl,
    clientId: 'attendance_App',
    responseType: 'code',
    scope: 'offline_access attendance',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44388',
      rootNamespace: 'ISG.attendance',
    },
  },
} as Environment;
