import { Environment } from '@abp/ng.core';

const baseUrl = 'https://attendance.fe.demo.egisg.com';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'attendance',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://attendance.demo.egisg.com/',
    redirectUri: baseUrl,
    clientId: 'attendance_App',
    responseType: 'code',
    scope: 'offline_access attendance',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://attendance.demo.egisg.com',
      rootNamespace: 'ISG.attendance',
    },
  },
} as Environment;
