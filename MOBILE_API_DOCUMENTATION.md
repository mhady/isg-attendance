# Mobile App API Documentation

This document describes the API endpoints needed for the Flutter mobile application.

## Base URL

```
https://your-domain.com/api
```

## Authentication

All API requests require authentication using Bearer token (JWT).

### Login

```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=password&
username={email}&
password={password}&
client_id=attendance_App&
scope=offline_access attendance
```

**Response:**
```json
{
  "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...",
  "token_type": "Bearer",
  "expires_in": 3600,
  "refresh_token": "..."
}
```

Use the `access_token` in subsequent requests:

```http
Authorization: Bearer {access_token}
```

## Mobile App Endpoints

### 1. Get Current User Info

```http
GET /api/account/my-profile
Authorization: Bearer {token}
```

**Response:**
```json
{
  "userName": "john.doe@company.com",
  "email": "john.doe@company.com",
  "name": "John",
  "surname": "Doe",
  "phoneNumber": "+1234567890",
  "isActive": true
}
```

### 2. Get Employee Details

```http
GET /api/attendance/employees/by-user/{userId}
Authorization: Bearer {token}
```

**Response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "fullName": "John Doe",
  "email": "john.doe@company.com",
  "phoneNumber": "+1234567890",
  "employeeCode": "EMP001",
  "locationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "locationName": "Main Office",
  "isActive": true
}
```

### 3. Get Company Settings

```http
GET /api/attendance/settings
Authorization: Bearer {token}
```

**Response:**
```json
{
  "normalWorkingHours": 8.0,
  "workdayStartTime": "08:00:00",
  "workdayEndTime": "17:00:00",
  "lateCheckInGracePeriodMinutes": 15,
  "earlyCheckOutGracePeriodMinutes": 15,
  "timeZone": "America/New_York"
}
```

### 4. Get Employees by Location

```http
GET /api/attendance/employees/by-location/{locationId}
Authorization: Bearer {token}
```

**Response:**
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "fullName": "John Doe",
    "employeeCode": "EMP001",
    "email": "john.doe@company.com",
    "locationName": "Main Office",
    "isActive": true
  },
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
    "fullName": "Jane Smith",
    "employeeCode": "EMP002",
    "email": "jane.smith@company.com",
    "locationName": "Main Office",
    "isActive": true
  }
]
```

### 5. Check In

```http
POST /api/attendance/attendances/check-in
Authorization: Bearer {token}
Content-Type: application/json

{
  "checkInTime": "2024-01-15T08:30:00Z",
  "notes": "Started work"
}
```

**Response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "employeeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "employeeName": "John Doe",
  "date": "2024-01-15",
  "checkInTime": "2024-01-15T08:30:00Z",
  "checkOutTime": null,
  "totalHours": 0,
  "overtimeHours": 0,
  "isFullDay": false,
  "notes": "Started work"
}
```

### 6. Check Out

```http
POST /api/attendance/attendances/check-out
Authorization: Bearer {token}
Content-Type: application/json

{
  "checkOutTime": "2024-01-15T17:30:00Z",
  "notes": "Finished work"
}
```

**Response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "employeeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "employeeName": "John Doe",
  "date": "2024-01-15",
  "checkInTime": "2024-01-15T08:30:00Z",
  "checkOutTime": "2024-01-15T17:30:00Z",
  "totalHours": 9.0,
  "overtimeHours": 1.0,
  "isFullDay": true,
  "notes": "Started work | Finished work"
}
```

### 7. Get Today's Attendance

```http
GET /api/attendance/attendances/today
Authorization: Bearer {token}
```

**Response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "employeeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "employeeName": "John Doe",
  "date": "2024-01-15",
  "checkInTime": "2024-01-15T08:30:00Z",
  "checkOutTime": null,
  "totalHours": 0,
  "overtimeHours": 0,
  "isFullDay": false
}
```

Returns `null` if no attendance record exists for today.

### 8. Get Attendance by Date Range

```http
GET /api/attendance/attendances/by-date-range?startDate=2024-01-01&endDate=2024-01-31&employeeId={employeeId}
Authorization: Bearer {token}
```

**Response:**
```json
{
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "employeeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "employeeName": "John Doe",
      "date": "2024-01-15",
      "checkInTime": "2024-01-15T08:30:00Z",
      "checkOutTime": "2024-01-15T17:30:00Z",
      "totalHours": 9.0,
      "overtimeHours": 1.0,
      "isFullDay": true
    }
  ]
}
```

## Mobile App Flow

### Initial Screen (Login)

1. User enters email and password
2. App calls `/connect/token` to get access token
3. Store token securely (e.g., using flutter_secure_storage)

### Home Screen

After successful login:

1. Call `GET /api/account/my-profile` to get user info
2. Call `GET /api/attendance/employees/by-user/{userId}` to get employee details
3. Call `GET /api/attendance/settings` to get company settings
4. Call `GET /api/attendance/attendances/today` to check if already checked in

Display:
- Company Name (from tenant info)
- Location Name (from employee details)
- Employee Name
- Welcome message
- Check-in/Check-out buttons based on current status

### View Other Employees

1. Get `locationId` from employee details
2. Call `GET /api/attendance/employees/by-location/{locationId}`
3. Display list of employees in the same location

### Check In/Check Out

**Check In:**
1. User taps "Check In" button
2. Get current timestamp
3. POST to `/api/attendance/attendances/check-in` with current time
4. Show success message
5. Refresh home screen to show checked-in status

**Check Out:**
1. User taps "Check Out" button
2. Get current timestamp
3. POST to `/api/attendance/attendances/check-out` with current time
4. Show success message with total hours worked
5. Refresh home screen

### View Past Attendance

1. User selects a date from date picker
2. If date is today, show today's attendance (with check-in/check-out buttons)
3. If date is in the past, call `/api/attendance/attendances/by-date-range`
4. Display attendance record (read-only, no check-in/check-out allowed)

## Error Handling

### Common Error Responses

**401 Unauthorized:**
```json
{
  "error": {
    "code": "Unauthorized",
    "message": "Authentication failed"
  }
}
```

**403 Forbidden:**
```json
{
  "error": {
    "code": "Forbidden",
    "message": "You don't have permission to perform this action"
  }
}
```

**400 Bad Request:**
```json
{
  "error": {
    "code": "BadRequest",
    "message": "Already checked in today",
    "validationErrors": []
  }
}
```

## Multi-Tenancy

The API automatically determines the tenant (company) based on:

1. The tenant resolution middleware in ABP Framework
2. Typically resolved from subdomain or HTTP header

For mobile app, you might use:
- Different base URLs per company: `https://company1.yourdomain.com`
- OR header-based: `__tenant: company-name`

Consult with backend team on tenant resolution strategy.

## Security Notes

1. **Always use HTTPS** in production
2. **Store tokens securely** using platform-specific secure storage
3. **Refresh tokens** before they expire
4. **Clear tokens** on logout
5. **Validate SSL certificates**
6. **Don't log sensitive data** (passwords, tokens)

## Rate Limiting

The API may have rate limiting enabled. Typical limits:
- 100 requests per minute per user
- 1000 requests per hour per user

If rate limit is exceeded, you'll receive a `429 Too Many Requests` response.

## Testing

### Test Credentials

For development/testing, you can use:

**System Admin:**
- Username: `admin`
- Password: `1q2w3E*`

**Company Admin:**
- Created by System Admin for each company

**Employee:**
- Created by Company Admin
- Credentials provided by admin

### Postman Collection

Import the Swagger JSON as a Postman collection for easier testing:

```
https://your-domain.com/swagger/v1/swagger.json
```
