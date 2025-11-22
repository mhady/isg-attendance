using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISG.attendance.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserIdOptionalAndAddEmployeeLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Make UserId nullable in Employees table
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AppEmployees",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: false);

            // Create EmployeeLocations table
            migrationBuilder.CreateTable(
                name: "AppEmployeeLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppEmployeeLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppEmployeeLocations_AppEmployees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AppEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppEmployeeLocations_AppLocations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "AppLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppEmployeeLocations_EmployeeId",
                table: "AppEmployeeLocations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppEmployeeLocations_LocationId",
                table: "AppEmployeeLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppEmployeeLocations_TenantId_EmployeeId_LocationId",
                table: "AppEmployeeLocations",
                columns: new[] { "TenantId", "EmployeeId", "LocationId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop EmployeeLocations table
            migrationBuilder.DropTable(
                name: "AppEmployeeLocations");

            // Revert UserId to non-nullable
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AppEmployees",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
