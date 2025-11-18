# ISG Attendance System - Mobile API Documentation

## Overview

The ISG Attendance mobile application is built on ASP.NET Core with ABP Framework, exposing RESTful APIs for employee attendance management.

- **Base URL:** `https://localhost:44325/api/attendance/`
- **Authentication:** OAuth 2.0 / OpenID Connect
- **Response Format:** JSON
- **Multi-Tenant:** Yes (tenant isolation enabled)

---

## Authentication

### Login (Get Access Token)

**Endpoint:** `POST /connect/token`

**Content-Type:** `application/x-www-form-urlencoded`

**Request Body:**
```
grant_type=password
client_id=attendance_App
username=admin
password=1q2w3E*
scope=offline_access attendance
```

**Response:**
```json
{
  "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6...",
  "token_type": "Bearer",
  "expires_in": 3600,
  "refresh_token": "CfDJ8...",
  "scope": "offline_access attendance"
}
```

### Refresh Token

**Endpoint:** `POST /connect/token`

**Request Body:**
```
grant_type=refresh_token
client_id=attendance_App
refresh_token={refresh_token}
```

### Required Headers for All API Calls

```
Authorization: Bearer {access_token}
Content-Type: application/json
Accept: application/json
```

---

## 1. Employee Management APIs

### 1.1 Get All Employees (Paginated)

**Endpoint:** `GET /api/attendance/employees`

**Permission:** `attendance.Employees`

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| skipCount | int | 0 | Records to skip |
| maxResultCount | int | 10 | Records to return |
| sorting | string | null | Sort field (e.g., "fullName asc") |

**Response:**
```json
{
  "totalCount": 100,
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
      "fullName": "John Doe",
      "email": "john.doe@company.com",
      "phoneNumber": "+1234567890",
      "employeeCode": "EMP001",
      "locationId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
      "locationName": "Main Office",
      "isActive": true,
      "creationTime": "2024-01-15T08:30:00Z"
    }
  ]
}
```

### 1.2 Get Employee by ID

**Endpoint:** `GET /api/attendance/employees/{id}`

**Permission:** `attendance.Employees`

### 1.3 Get Employee by User ID

**Endpoint:** `GET /api/attendance/employees/by-user/{userId}`

**Permission:** `attendance.Employees`

**Use Case:** Get employee profile for logged-in user

### 1.4 Get Employees by Location

**Endpoint:** `GET /api/attendance/employees/by-location/{locationId}`

**Permission:** `attendance.Employees`

### 1.5 Create Employee

**Endpoint:** `POST /api/attendance/employees`

**Permission:** `attendance.Employees.Create`

**Request Body:**
```json
{
  "fullName": "John Doe",
  "email": "john.doe@company.com",
  "password": "SecureP@ss123",
  "phoneNumber": "+1234567890",
  "employeeCode": "EMP001",
  "locationId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
  "isActive": true
}
```

**Validation:**
- `fullName`: Required, max 256 characters
- `email`: Required, valid email format
- `password`: Required, min 6 characters

### 1.6 Update Employee

**Endpoint:** `PUT /api/attendance/employees/{id}`

**Permission:** `attendance.Employees.Edit`

**Request Body:**
```json
{
  "fullName": "John Doe Updated",
  "email": "john.doe.new@company.com",
  "phoneNumber": "+0987654321",
  "employeeCode": "EMP001-A",
  "locationId": "3fa85f64-5717-4562-b3fc-2c963f66afa9",
  "isActive": true
}
```

### 1.7 Delete Employee

**Endpoint:** `DELETE /api/attendance/employees/{id}`

**Permission:** `attendance.Employees.Delete`

**Response:** 204 No Content

### 1.8 Import Employees (Bulk)

**Endpoint:** `POST /api/attendance/employees/import`

**Permission:** `attendance.Employees.Import`

**Request Body:**
```json
[
  {
    "fullName": "Jane Smith",
    "email": "jane.smith@company.com",
    "password": "TempP@ss123",
    "phoneNumber": "+1111111111",
    "employeeCode": "EMP002",
    "locationName": "Branch Office"
  },
  {
    "fullName": "Bob Wilson",
    "email": "bob.wilson@company.com",
    "password": "TempP@ss456",
    "phoneNumber": "+2222222222",
    "employeeCode": "EMP003",
    "locationName": "Main Office"
  }
]
```

---

## 2. Location Management APIs

### 2.1 Get All Locations (Paginated)

**Endpoint:** `GET /api/attendance/locations`

**Permission:** `attendance.Locations`

**Response:**
```json
{
  "totalCount": 10,
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
      "name": "Main Office",
      "description": "Company headquarters",
      "isActive": true,
      "employeeCount": 50,
      "creationTime": "2024-01-01T00:00:00Z"
    }
  ]
}
```

### 2.2 Get All Locations (Non-Paginated)

**Endpoint:** `GET /api/attendance/locations/all`

**Permission:** `attendance.Locations`

**Use Case:** Dropdown lists, mobile app location selection

### 2.3 Get Location by ID

**Endpoint:** `GET /api/attendance/locations/{id}`

**Permission:** `attendance.Locations`

### 2.4 Create Location

**Endpoint:** `POST /api/attendance/locations`

**Permission:** `attendance.Locations.Create`

**Request Body:**
```json
{
  "name": "New Branch",
  "description": "New branch office downtown",
  "isActive": true
}
```

### 2.5 Update Location

**Endpoint:** `PUT /api/attendance/locations/{id}`

**Permission:** `attendance.Locations.Edit`

### 2.6 Delete Location

**Endpoint:** `DELETE /api/attendance/locations/{id}`

**Permission:** `attendance.Locations.Delete`

---

## 3. Attendance Management APIs

### 3.1 Check In (Mobile Primary Action)

**Endpoint:** `POST /api/attendance/attendances/check-in`

**Permission:** `attendance.Attendances.CheckIn`

**Request Body:**
```json
{
  "checkInTime": "2024-11-18T08:00:00Z",
  "notes": "Arrived on time"
}
```

**Response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afb0",
  "employeeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "employeeName": "John Doe",
  "employeeCode": "EMP001",
  "date": "2024-11-18T00:00:00Z",
  "checkInTime": "2024-11-18T08:00:00Z",
  "checkOutTime": null,
  "totalHours": 0,
  "overtimeHours": 0,
  "isFullDay": false,
  "notes": "Arrived on time"
}
```

**Notes:**
- Automatically creates attendance record for current employee
- Uses authenticated user to find employee
- Checks grace period for late arrivals

### 3.2 Check Out (Mobile Primary Action)

**Endpoint:** `POST /api/attendance/attendances/check-out`

**Permission:** `attendance.Attendances.CheckOut`

**Request Body:**
```json
{
  "checkOutTime": "2024-11-18T17:00:00Z",
  "notes": "Completed all tasks"
}
```

**Response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afb0",
  "employeeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "employeeName": "John Doe",
  "employeeCode": "EMP001",
  "date": "2024-11-18T00:00:00Z",
  "checkInTime": "2024-11-18T08:00:00Z",
  "checkOutTime": "2024-11-18T17:00:00Z",
  "totalHours": 9.0,
  "overtimeHours": 1.0,
  "isFullDay": true,
  "notes": "Completed all tasks"
}
```

**Automatic Calculations:**
- `totalHours`: Difference between check-out and check-in
- `overtimeHours`: Hours exceeding normal working hours
- `isFullDay`: true if totalHours >= company's normalWorkingHours

### 3.3 Get Today's Attendance

**Endpoint:** `GET /api/attendance/attendances/today`

**Permission:** `attendance.Attendances.View`

**Use Case:** Mobile app home screen status

### 3.4 Get All Attendance Records

**Endpoint:** `GET /api/attendance/attendances`

**Permission:** `attendance.Attendances.View`

### 3.5 Get Attendance by Employee

**Endpoint:** `GET /api/attendance/attendances/by-employee/{employeeId}`

**Permission:** `attendance.Attendances.View`

### 3.6 Get Attendance by Date

**Endpoint:** `GET /api/attendance/attendances/by-date`

**Permission:** `attendance.Attendances.View`

**Query Parameters:**
- `date`: Required (e.g., 2024-11-18)

### 3.7 Get Attendance by Date Range

**Endpoint:** `GET /api/attendance/attendances/by-date-range`

**Permission:** `attendance.Attendances.View`

**Query Parameters:**
- `startDate`: Required
- `endDate`: Required
- `employeeId`: Optional (filter by employee)

---

## 4. Company Settings APIs

### 4.1 Get Company Settings

**Endpoint:** `GET /api/attendance/settings`

**Permission:** `attendance.Settings`

**Response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afc0",
  "normalWorkingHours": 8.0,
  "workdayStartTime": "08:00:00",
  "workdayEndTime": "17:00:00",
  "lateCheckInGracePeriodMinutes": 15,
  "earlyCheckOutGracePeriodMinutes": 15,
  "timeZone": "UTC"
}
```

### 4.2 Update Company Settings

**Endpoint:** `POST /api/attendance/settings`

**Permission:** `attendance.Settings.Manage`

**Request Body:**
```json
{
  "normalWorkingHours": 8.0,
  "workdayStartTime": "09:00:00",
  "workdayEndTime": "18:00:00",
  "lateCheckInGracePeriodMinutes": 10,
  "earlyCheckOutGracePeriodMinutes": 10,
  "timeZone": "Asia/Dubai"
}
```

---

## 5. Reports APIs

### 5.1 Get Location Summary Report

**Endpoint:** `GET /api/attendance/reports/location-summary`

**Permission:** `attendance.Reports.LocationSummary`

**Response:**
```json
[
  {
    "locationId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
    "locationName": "Main Office",
    "totalEmployees": 50,
    "activeEmployees": 48,
    "presentToday": 45
  },
  {
    "locationId": "3fa85f64-5717-4562-b3fc-2c963f66afa9",
    "locationName": "Branch Office",
    "totalEmployees": 20,
    "activeEmployees": 18,
    "presentToday": 16
  }
]
```

### 5.2 Get Monthly Attendance Report

**Endpoint:** `POST /api/attendance/reports/monthly-attendance`

**Permission:** `attendance.Reports.MonthlyAttendance`

**Request Body:**
```json
{
  "employeeId": null,
  "employeeName": null,
  "month": 11,
  "year": 2024
}
```

**Response:**
```json
[
  {
    "employeeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "employeeName": "John Doe",
    "employeeCode": "EMP001",
    "locationName": "Main Office",
    "year": 2024,
    "month": 11,
    "totalWorkingDays": 22,
    "fullWorkingDays": 20,
    "partialWorkingDays": 1,
    "absentDays": 1,
    "totalHours": 168.5,
    "totalOvertimeHours": 8.5
  }
]
```

---

## Error Handling

### HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | Success |
| 201 | Created |
| 204 | No Content |
| 400 | Bad Request (validation error) |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 500 | Internal Server Error |

### Error Response Format

```json
{
  "error": {
    "code": "attendance:employee_not_found",
    "message": "Employee not found with the given ID.",
    "details": "The employee with ID '3fa85f64-5717-4562-b3fc-2c963f66afa6' does not exist."
  }
}
```

---

## Mobile App Flow

### 1. Login Flow
1. Call `POST /connect/token` with credentials
2. Store access_token and refresh_token securely
3. Call `GET /api/attendance/employees/by-user/{userId}` to get employee profile

### 2. Check-In Flow
1. Call `GET /api/attendance/attendances/today` to check current status
2. If no check-in, show "Check In" button
3. Call `POST /api/attendance/attendances/check-in`
4. Update UI with response

### 3. Check-Out Flow
1. Call `GET /api/attendance/attendances/today` to verify check-in exists
2. Call `POST /api/attendance/attendances/check-out`
3. Display total hours worked

### 4. View History Flow
1. Call `GET /api/attendance/attendances/by-date-range` with date range
2. Display attendance records in list

---

## Permission Summary

| Role | Permissions |
|------|-------------|
| **System Admin** | All permissions |
| **Company Admin** | Employees.*, Locations.*, Attendances.*, Settings.Manage, Reports.* |
| **Employee** | Attendances.CheckIn, Attendances.CheckOut, Attendances.ViewOwn |

---

## Notes for Mobile Developers

1. **Token Expiration**: Access tokens expire in 1 hour. Implement automatic refresh.
2. **Timezone**: All times are in UTC. Convert to local timezone for display.
3. **Offline Support**: Cache today's attendance status for offline viewing.
4. **Push Notifications**: Implement reminders for check-in/check-out times.
5. **Location Services**: Consider adding GPS verification for check-in/out.
