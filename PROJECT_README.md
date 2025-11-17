# ISG Attendance Management System

A comprehensive multi-tenant attendance management system built with ABP Framework, Angular, and designed for mobile app integration.

## 🎯 Features

### System Roles

1. **System Admin (Super Admin)**
   - Create, edit, and delete companies (tenants)
   - Create Company Admin users for each company
   - Full system oversight

2. **Company Admin**
   - User/Employee management (CRUD)
   - Excel import for bulk employee creation
   - Location management
   - View attendance records
   - Access to reports
   - Configure company settings

3. **Employee** (Mobile App Users)
   - Check-in/Check-out functionality
   - View own attendance history
   - View other employees in same location
   - Mobile app access only

### Admin Panel Capabilities

#### User Management
- ✅ Add/Edit/Delete employees
- ✅ Set email and password for each user
- ✅ Import employees from Excel
- ✅ Assign employees to locations

#### Location Management
- ✅ CRUD operations for locations
- ✅ Assign employees to locations
- ✅ Track employee count per location

#### Attendance Management
- ✅ View all employee attendance records
- ✅ Check-in and check-out times
- ✅ Total worked hours calculation
- ✅ Overtime hours calculation
- ✅ Filter by employee, date, or location

#### Settings
- ✅ Normal working hours per day
- ✅ Workday start time
- ✅ Workday end time
- ✅ Late check-in grace period
- ✅ Early check-out grace period
- ✅ Company timezone

#### Reports
1. **Location Summary Report**
   - All locations with employee count
   - Active employees per location
   - Present today count

2. **Monthly Attendance Report**
   - Searchable by employee name and month
   - Full working days count
   - Overtime days count
   - Total hours and overtime hours
   - Absent days calculation

### Mobile App Features

#### Home Screen
- ✅ Display company name
- ✅ Display location name
- ✅ Display employee name
- ✅ Welcome message
- ✅ Check-in/Check-out buttons

#### Attendance
- ✅ Check-in for current day
- ✅ Check-out for current day
- ✅ View today's attendance status
- ✅ Automatic hours calculation

#### Employee List
- ✅ View employees in same location
- ✅ Filter by location

#### History
- ✅ Select and view past attendance
- ✅ Read-only view for previous days
- ✅ Current day allows check-in/check-out

## 🏗️ Architecture

### Technology Stack

**Backend:**
- ABP Framework (.NET 8.0)
- Entity Framework Core
- PostgreSQL Database
- ASP.NET Core Identity
- Multi-tenancy support
- RESTful API

**Frontend:**
- Angular (standalone components)
- ABP Angular UI
- TypeScript
- RxJS
- ng-bootstrap

**Mobile:**
- Flutter (recommended)
- API-first design
- JWT authentication

### Project Structure

```
isg-attendance/
├── aspnet-core/                 # Backend .NET solution
│   ├── src/
│   │   ├── ISG.attendance.Domain/              # Domain entities
│   │   │   └── Entities/                       # Employee, Location, Attendance, CompanySettings
│   │   ├── ISG.attendance.Domain.Shared/       # Shared domain logic
│   │   ├── ISG.attendance.Application/         # Application services
│   │   │   └── Services/                       # Business logic services
│   │   ├── ISG.attendance.Application.Contracts/ # DTOs and interfaces
│   │   │   ├── DTOs/                           # Data Transfer Objects
│   │   │   ├── Permissions/                    # Permission definitions
│   │   │   └── Services/                       # Service interfaces
│   │   ├── ISG.attendance.EntityFrameworkCore/ # EF Core configuration
│   │   ├── ISG.attendance.HttpApi/             # API controllers
│   │   └── ISG.attendance.HttpApi.Host/        # API host application
│   └── test/                    # Unit and integration tests
├── angular/                     # Angular frontend
│   └── src/
│       └── app/
│           ├── proxy/          # API proxy services
│           │   └── attendance/ # Attendance module services
│           └── ...
└── docs/                       # Documentation
```

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Node.js 18+ and npm/yarn
- PostgreSQL 12+
- Angular CLI
- (Optional) Flutter SDK for mobile app

### Backend Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd isg-attendance
   ```

2. **Configure database connection**

   Edit `aspnet-core/src/ISG.attendance.HttpApi.Host/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "Default": "Host=localhost;Port=5432;Database=attendance;User ID=postgres;Password=yourpassword;"
     }
   }
   ```

3. **Create database migration**
   ```bash
   cd aspnet-core/src/ISG.attendance.EntityFrameworkCore
   dotnet ef migrations add InitialCreate --startup-project ../ISG.attendance.HttpApi.Host
   ```

4. **Apply migration**
   ```bash
   dotnet ef database update --startup-project ../ISG.attendance.HttpApi.Host
   ```

   Or run the DbMigrator:
   ```bash
   cd aspnet-core/src/ISG.attendance.DbMigrator
   dotnet run
   ```

5. **Run the backend**
   ```bash
   cd aspnet-core/src/ISG.attendance.HttpApi.Host
   dotnet run
   ```

   API will be available at:
   - HTTPS: https://localhost:44388
   - Swagger: https://localhost:44388/swagger

### Frontend Setup

1. **Navigate to Angular directory**
   ```bash
   cd angular
   ```

2. **Install dependencies**
   ```bash
   yarn install
   # or
   npm install
   ```

3. **Update environment configuration**

   Edit `src/environments/environment.ts` if needed.

4. **Run the application**
   ```bash
   yarn start
   # or
   npm start
   ```

   Application will be available at: http://localhost:4200

### Default Credentials

**System Admin:**
- Username: `admin`
- Password: `1q2w3E*`

## 📱 Mobile App Integration

The backend provides RESTful APIs for mobile app integration. See [MOBILE_API_DOCUMENTATION.md](MOBILE_API_DOCUMENTATION.md) for detailed API documentation.

### Key Mobile Endpoints

- **Authentication**: `POST /connect/token`
- **Check-in**: `POST /api/attendance/attendances/check-in`
- **Check-out**: `POST /api/attendance/attendances/check-out`
- **Get Today's Attendance**: `GET /api/attendance/attendances/today`
- **Get Employee Info**: `GET /api/attendance/employees/by-user/{userId}`
- **Get Location Employees**: `GET /api/attendance/employees/by-location/{locationId}`

## 📊 Database Schema

### Core Entities

**Location**
- Id (Guid, PK)
- TenantId (Guid, nullable) - Multi-tenancy
- Name (string, required)
- Description (string)
- IsActive (bool)

**Employee**
- Id (Guid, PK)
- TenantId (Guid, nullable) - Multi-tenancy
- UserId (Guid) - Links to Identity User
- FullName (string, required)
- Email (string, required)
- PhoneNumber (string)
- EmployeeCode (string)
- LocationId (Guid, FK)
- IsActive (bool)

**Attendance**
- Id (Guid, PK)
- TenantId (Guid, nullable) - Multi-tenancy
- EmployeeId (Guid, FK)
- Date (DateTime)
- CheckInTime (DateTime)
- CheckOutTime (DateTime)
- TotalHours (double)
- OvertimeHours (double)
- IsFullDay (bool)
- Notes (string)

**CompanySettings**
- Id (Guid, PK)
- TenantId (Guid, nullable) - Multi-tenancy, unique
- NormalWorkingHours (double)
- WorkdayStartTime (TimeSpan)
- WorkdayEndTime (TimeSpan)
- LateCheckInGracePeriodMinutes (int)
- EarlyCheckOutGracePeriodMinutes (int)
- TimeZone (string)

## 🔐 Permissions

### Permission Structure

```
attendance
├── Employees
│   ├── Default (View)
│   ├── Create
│   ├── Edit
│   ├── Delete
│   └── Import
├── Locations
│   ├── Default (View)
│   ├── Create
│   ├── Edit
│   └── Delete
├── Attendances
│   ├── Default (View)
│   ├── View (View All)
│   ├── ViewOwn (View Own Only)
│   ├── CheckIn
│   └── CheckOut
├── Settings
│   ├── Default (View)
│   └── Manage
└── Reports
    ├── Default (View)
    ├── LocationSummary
    └── MonthlyAttendance
```

### Role-Permission Mapping

**System Admin:**
- All ABP built-in permissions
- Tenant management

**Company Admin:**
- All attendance module permissions
- Cannot access other tenants

**Employee:**
- attendance.Attendances.CheckIn
- attendance.Attendances.CheckOut
- attendance.Attendances.ViewOwn

## 🎨 UI Components (To Be Implemented)

### Modules to Create in Angular

1. **Employee Management Module**
   - Employee list with pagination
   - Employee create/edit forms
   - Excel import functionality
   - Location assignment

2. **Location Management Module**
   - Location list
   - Location create/edit forms
   - Employee count display

3. **Attendance View Module**
   - Attendance list with filters
   - Date range selector
   - Employee filter
   - Export functionality

4. **Settings Module**
   - Company settings form
   - Time configuration
   - Grace period settings

5. **Reports Module**
   - Location summary dashboard
   - Monthly attendance report
   - Search and filter options
   - Export to Excel

## 📝 Excel Import Template

For employee import, use the following Excel structure:

| FullName | Email | Password | PhoneNumber | EmployeeCode | LocationName |
|----------|-------|----------|-------------|--------------|--------------|
| John Doe | john@company.com | Pass123! | +1234567890 | EMP001 | Main Office |
| Jane Smith | jane@company.com | Pass123! | +1234567891 | EMP002 | Branch Office |

## 🔄 Multi-Tenancy

This application uses ABP Framework's multi-tenancy feature:

- Each company is a separate **tenant**
- Data is **automatically isolated** per tenant
- System Admin can manage all tenants
- Company Admin can only access their own tenant's data
- Employees belong to a specific tenant

### Tenant Resolution

Tenants can be resolved by:
- Subdomain: `company1.yourdomain.com`
- Header: `__tenant: company-name`
- Query parameter: `?__tenant=company-name`

## 🧪 Testing

### Unit Tests

```bash
cd aspnet-core/test/ISG.attendance.Application.Tests
dotnet test
```

### Integration Tests

```bash
cd aspnet-core/test/ISG.attendance.EntityFrameworkCore.Tests
dotnet test
```

## 📚 Additional Documentation

- [Backend Setup Guide](BACKEND_SETUP.md)
- [Mobile API Documentation](MOBILE_API_DOCUMENTATION.md)
- [ABP Framework Documentation](https://docs.abp.io)

## 🤝 Contributing

1. Create a feature branch
2. Make your changes
3. Write tests
4. Submit a pull request

## 📄 License

[Your License Here]

## 🆘 Support

For issues and questions:
- Create an issue in the repository
- Contact the development team

## 🗺️ Roadmap

### Phase 1 (Current)
- ✅ Backend API implementation
- ✅ Multi-tenancy setup
- ✅ Core entities and services
- ✅ Permission system
- ✅ API documentation

### Phase 2 (Next)
- ⬜ Angular UI components
- ⬜ Employee management UI
- ⬜ Location management UI
- ⬜ Attendance viewing UI
- ⬜ Reports implementation
- ⬜ Settings page

### Phase 3 (Future)
- ⬜ Flutter mobile app
- ⬜ Push notifications
- ⬜ Geolocation check-in
- ⬜ Face recognition
- ⬜ Advanced analytics
- ⬜ Export to various formats
