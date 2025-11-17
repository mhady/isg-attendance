# Backend Setup Guide

## Prerequisites
- .NET 8.0 SDK or later
- PostgreSQL database
- Entity Framework Core CLI tools

## Database Setup

### 1. Update Connection String
Edit `aspnet-core/src/ISG.attendance.HttpApi.Host/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=attendance;User ID=your_user;Password=your_password;"
  }
}
```

### 2. Create Database Migration

Navigate to the EntityFrameworkCore project:

```bash
cd aspnet-core/src/ISG.attendance.EntityFrameworkCore
```

Add a new migration:

```bash
dotnet ef migrations add InitialCreate --startup-project ../ISG.attendance.HttpApi.Host
```

### 3. Apply Migration

Update the database:

```bash
dotnet ef database update --startup-project ../ISG.attendance.HttpApi.Host
```

Or run the DbMigrator project:

```bash
cd aspnet-core/src/ISG.attendance.DbMigrator
dotnet run
```

## Running the Backend

### 1. Navigate to the Host project:

```bash
cd aspnet-core/src/ISG.attendance.HttpApi.Host
```

### 2. Run the application:

```bash
dotnet run
```

The API will be available at:
- HTTPS: https://localhost:44388
- Swagger UI: https://localhost:44388/swagger

## Initial Setup

### 1. Create System Admin User

The default admin user should be created automatically:
- Username: `admin`
- Password: `1q2w3E*`

### 2. Create Your First Tenant (Company)

1. Login as system admin
2. Navigate to Tenant Management
3. Create a new tenant (company)
4. Create a Company Admin user for the tenant

### 3. Configure Company Settings

1. Login as Company Admin
2. Navigate to Settings
3. Configure:
   - Normal working hours (default: 8.0)
   - Workday start time (default: 08:00)
   - Workday end time (default: 17:00)
   - Grace periods

## Multi-Tenancy

This application uses ABP Framework's multi-tenancy feature:

- Each company is a separate tenant
- Data is automatically isolated per tenant
- System Admin can manage all tenants
- Company Admin can only manage their own tenant's data

## API Endpoints

### Locations
- `GET /api/attendance/locations` - Get all locations
- `POST /api/attendance/locations` - Create location
- `PUT /api/attendance/locations/{id}` - Update location
- `DELETE /api/attendance/locations/{id}` - Delete location

### Employees
- `GET /api/attendance/employees` - Get all employees
- `POST /api/attendance/employees` - Create employee
- `PUT /api/attendance/employees/{id}` - Update employee
- `DELETE /api/attendance/employees/{id}` - Delete employee
- `POST /api/attendance/employees/import` - Import from Excel
- `GET /api/attendance/employees/by-location/{locationId}` - Get by location

### Attendance
- `GET /api/attendance/attendances` - Get all attendance records
- `POST /api/attendance/attendances/check-in` - Check in
- `POST /api/attendance/attendances/check-out` - Check out
- `GET /api/attendance/attendances/today` - Get today's attendance
- `GET /api/attendance/attendances/by-employee/{employeeId}` - Get by employee
- `GET /api/attendance/attendances/by-date` - Get by date

### Settings
- `GET /api/attendance/settings` - Get company settings
- `POST /api/attendance/settings` - Create/Update settings

### Reports
- `GET /api/attendance/reports/location-summary` - Location summary
- `POST /api/attendance/reports/monthly-attendance` - Monthly attendance report

## Troubleshooting

### Migration Issues

If you encounter migration issues:

1. Delete all migrations in `Migrations` folder
2. Drop the database
3. Create a fresh migration
4. Apply the migration

### Connection Issues

Ensure PostgreSQL is running and the connection string is correct.

### Permission Issues

Make sure you're logged in with the correct role:
- System Admin: Tenant management
- Company Admin: All company operations
- Employee: Check-in/check-out only
