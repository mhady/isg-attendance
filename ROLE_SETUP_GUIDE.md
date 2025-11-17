# Attendance System - Role Setup and Company Management Guide

## Overview

The attendance system has a three-tier role structure with proper data isolation:

1. **System Admin (admin)** - Can manage tenants (companies) and create Company Admin users
2. **Company Admin (CompanyAdmin)** - Can manage their company's data (employees, locations, settings, reports)
3. **Employee** - Mobile app users who can check in/out and view their own attendance

## Role Permissions

### System Admin (admin role)
- Full system access
- Can create and manage tenants (companies)
- Can create users with any role
- **Does NOT have direct access to company data** (employees, locations, etc.) - this is intentional for security

### Company Admin (CompanyAdmin role)
- Full access to their company's data:
  - ✅ Employee Management (Create, Edit, Delete, Import)
  - ✅ Location Management (Create, Edit, Delete)
  - ✅ Attendance Viewing (View all employees' attendance)
  - ✅ Company Settings (Manage working hours, grace periods)
  - ✅ Reports (Location Summary, Monthly Attendance)
- ❌ Cannot create or manage other companies
- ❌ Cannot create other Company Admin users (only System Admin can)
- ❌ Cannot check in/out (this is for employees only)

### Employee Role
- ✅ Check In/Out (mobile app only, current day only)
- ✅ View own attendance history
- ❌ Cannot view other employees' data
- ❌ Cannot manage any company data

## Data Isolation

All data tables automatically filter by company (tenant):
- **Employee** - includes TenantId (company ID)
- **Location** - includes TenantId (company ID)
- **Attendance** - includes TenantId (company ID)
- **CompanySettings** - includes TenantId (company ID)

When a Company Admin logs in, they **only see their company's data**. ABP Framework handles this automatically through multi-tenancy.

## Step-by-Step Setup Guide

### Step 1: Run Database Migrations

First, apply the data seeder to create roles and permissions:

```bash
cd aspnet-core/src/ISG.attendance.DbMigrator
dotnet run
```

This will:
- Create the database schema
- Create **CompanyAdmin** and **Employee** roles
- Assign proper permissions to each role
- Seed initial admin user

### Step 2: Login as System Admin

1. Start the backend:
   ```bash
   cd aspnet-core/src/ISG.attendance.HttpApi.Host
   dotnet run
   ```

2. Start the Angular frontend:
   ```bash
   cd angular
   npm start
   ```

3. Open browser: `http://localhost:4200`

4. Login with default admin credentials:
   - **Username**: `admin`
   - **Password**: `1q2w3E*` (default ABP password)

### Step 3: Create a Company (Tenant)

As System Admin:

1. In the left menu, navigate to **Administration → Tenants**
2. Click **New Tenant** button
3. Fill in:
   - **Name**: Company short name (e.g., "acme-corp")
   - **Admin Email**: Email for the company admin user
   - **Admin Password**: Password for the company admin user
4. Click **Save**

The tenant (company) is now created!

### Step 4: Assign Company Admin Role

After creating the tenant:

1. Still as System Admin, switch to the tenant context:
   - Look for **Current Tenant** dropdown in the top menu
   - Select the company you just created

2. Navigate to **Administration → Identity Management → Users**
3. Click on the admin user that was created with the tenant
4. Go to the **Roles** tab
5. Assign the **CompanyAdmin** role
6. Remove the **admin** role (unless you want them to also be a system admin)
7. Click **Save**

### Step 5: Login as Company Admin

1. Logout from System Admin
2. Login with the Company Admin credentials you created in Step 3
3. **Important**: Before logging in, select the correct tenant:
   - On the login page, look for **Tenant** field
   - Enter the company name (e.g., "acme-corp")
   - Then enter username and password

4. After login, you'll see the **Attendance** menu with:
   - Employees
   - Locations
   - Attendances
   - Settings
   - Reports

### Step 6: Create Employees

As Company Admin:

1. Navigate to **Attendance → Employees**
2. Click **New Employee**
3. Fill in employee details:
   - Full Name
   - Email
   - Employee Code
   - Password (for their login)
   - Phone Number
   - Location (optional)

4. The system automatically:
   - Creates an Identity user for the employee
   - Assigns the **Employee** role
   - Links the employee to your company (tenant)

### Step 7: Create Locations

As Company Admin:

1. Navigate to **Attendance → Locations**
2. Click **New Location**
3. Fill in:
   - Name (e.g., "Main Office", "Warehouse")
   - Description
   - Active status

4. Assign employees to locations by editing employees and selecting the location

### Step 8: Configure Company Settings

As Company Admin:

1. Navigate to **Attendance → Settings**
2. Configure:
   - **Normal Working Hours**: e.g., 8.0 hours
   - **Grace Period (Minutes)**: e.g., 15 minutes
   - **Timezone**: Your company's timezone

These settings control:
- When overtime starts (after normal working hours)
- Check-in grace period
- Date/time calculations

### Step 9: Employee Mobile Usage

Employees will use a mobile app (Flutter) to:

1. **Login** with their credentials
   - Must select their tenant/company name
   - Use credentials created in Step 6

2. **Check In** (start of workday)
   - Tap "Check In"
   - System records check-in time
   - Only allowed for current day

3. **Check Out** (end of workday)
   - Tap "Check Out"
   - System calculates:
     - Total hours worked
     - Overtime hours (if any)
     - Full day status

4. **View Own Attendance**
   - See their check-in/out history
   - View total hours and overtime
   - **Cannot edit past records** (read-only)

## Important Notes

### Multi-Tenancy Rules

- Each company (tenant) is completely isolated
- Company Admins can ONLY see and manage their company's data
- Employees can ONLY see their own attendance records
- System Admin can manage all tenants but cannot see company data directly

### User Creation Workflow

```
System Admin → Creates Tenant → Creates Tenant Admin User → Assigns CompanyAdmin Role

Company Admin → Creates Employees → System assigns Employee Role
```

### Permission Enforcement

- All API endpoints check permissions before allowing actions
- Angular routes are protected by `requiredPolicy`
- Multi-tenancy is enforced at the database level

### Data Seeder

The `attendanceDataSeedContributor` runs automatically when you run the DbMigrator. It:
- Creates **CompanyAdmin** role with full company management permissions
- Creates **Employee** role with limited check-in/out permissions
- Does NOT modify the existing **admin** role (System Admin)

## Troubleshooting

### "I can't see the Attendance menu"

**Solution**: Make sure you're logged in with a user that has the correct role:
- Company Admin should see all Attendance menu items
- Employee should only see limited items (if any via web, mobile is primary)
- System Admin won't see Attendance menu (they manage tenants)

### "I can't create a company"

**Solution**: Only System Admin can create tenants. Login as `admin` user and go to **Administration → Tenants**.

### "I can see other companies' data"

**Solution**: This should NEVER happen. If it does, there's a bug. Make sure:
- You're logging in with the correct tenant name
- Your user is properly assigned to a tenant
- Report this as a critical security issue

### "Employee can't login"

**Solution**: Make sure:
1. Employee was created properly (check **Administration → Users** as Company Admin)
2. Employee has **Employee** role assigned
3. Employee is entering the correct **tenant name** on login
4. Password is correct

## API Endpoints for Mobile App

See `MOBILE_API_DOCUMENTATION.md` for complete API reference.

Key endpoints:
- `POST /api/attendance/attendances/check-in` - Check in for current day
- `POST /api/attendance/attendances/check-out` - Check out
- `GET /api/attendance/attendances/my-attendance` - View own attendance history

All endpoints require authentication and automatically filter by tenant.

## Next Steps

1. Run the database migrator to create roles
2. Login as System Admin
3. Create your first company (tenant)
4. Assign Company Admin role to the tenant admin user
5. Login as Company Admin
6. Start creating employees and locations
7. Configure company settings
8. Employees can start using the mobile app!
