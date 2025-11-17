using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using ISG.attendance.Entities;

namespace ISG.attendance.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class attendanceDbContext :
    AbpDbContext<attendanceDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    // Custom Entities
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<CompanySettings> CompanySettings { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public attendanceDbContext(DbContextOptions<attendanceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        builder.Entity<Location>(b =>
        {
            b.ToTable(attendanceConsts.DbTablePrefix + "Locations", attendanceConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(256);
            b.Property(x => x.Description).HasMaxLength(1024);

            b.HasIndex(x => new { x.TenantId, x.Name });
        });

        builder.Entity<Employee>(b =>
        {
            b.ToTable(attendanceConsts.DbTablePrefix + "Employees", attendanceConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.FullName).IsRequired().HasMaxLength(256);
            b.Property(x => x.Email).IsRequired().HasMaxLength(256);
            b.Property(x => x.PhoneNumber).HasMaxLength(50);
            b.Property(x => x.EmployeeCode).HasMaxLength(50);

            b.HasOne(x => x.Location)
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => new { x.TenantId, x.UserId });
            b.HasIndex(x => new { x.TenantId, x.EmployeeCode });
        });

        builder.Entity<Attendance>(b =>
        {
            b.ToTable(attendanceConsts.DbTablePrefix + "Attendances", attendanceConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Notes).HasMaxLength(1024);

            b.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => new { x.TenantId, x.EmployeeId, x.Date });
        });

        builder.Entity<CompanySettings>(b =>
        {
            b.ToTable(attendanceConsts.DbTablePrefix + "CompanySettings", attendanceConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.TimeZone).HasMaxLength(100);

            b.HasIndex(x => x.TenantId).IsUnique();
        });
    }
}
