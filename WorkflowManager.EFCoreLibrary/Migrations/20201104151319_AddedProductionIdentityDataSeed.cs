using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkflowManager.EFCoreLibrary.Migrations
{
    public partial class AddedProductionIdentityDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "DisplayName", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "admin", "3245bae1-84e3-4b19-9f98-c6df51bb5e56", "Administrator", "Admin", "ADMIN" },
                    { "technician", "9d4436e9-cc6d-4f29-83b3-b6f1857dba98", "Technik", "Technician", "TECHNICIAN" },
                    { "manager", "81b885d0-6d68-4559-8300-3606ae6eb8ea", "Menadżer", "Manager", "MANAGER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BuildingId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "Lock", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin", 0, null, "c27b28bc-3a93-40e7-973b-e7b7ad9a48c0", null, false, null, null, false, false, null, null, "ADMIN", "AQAAAAEAACcQAAAAEIgHpomH+aQM4ewCjcgjoxnQ5GxPHrUu9KgWFysP6hezLtgAXTaCBuFZd2F5N3BbgA==", null, false, "1a4e30de-6635-4adc-a653-b5415238ae99", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "admin", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "manager");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "admin", "admin" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin");
        }
    }
}
