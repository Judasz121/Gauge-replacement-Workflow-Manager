using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkflowManager.EFCoreLibrary.Migrations
{
    public partial class RemovedCostMeterMeasurement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Measurement",
                table: "CostMeters");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "35bc847b-13e0-465d-900c-bc7031f651e5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "manager",
                column: "ConcurrencyStamp",
                value: "a4279c39-608a-4917-a942-e43095f8391b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "5fc51ef4-1689-424f-a784-e61413809509");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "00c6e47a-e5b4-490c-924a-03652a496f99", "AQAAAAEAACcQAAAAEEXSrNKzvco/HUg9gQ3BjAywtxg7wm44yUEhstIfiUvhUsfKte2jleBR5jUW2gLZVA==", "638106be-6ceb-44f7-9984-bdb383def721" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Measurement",
                table: "CostMeters",
                type: "real",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "e52d8b7f-9523-4d6f-bfef-a92a68d056f3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "manager",
                column: "ConcurrencyStamp",
                value: "9eea87dd-6c9d-4b10-97e2-de022b70edad");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "f18fda3d-067c-407e-910c-1f49fbef4a11");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "38c9e681-e3b4-4ffd-9ed3-a593eddd0e60", "AQAAAAEAACcQAAAAEHAEC9PUHbwYS6t8N7Svq6Y7q0F7glaKaNgEL24orVHiyr75a67/Me7mBtvoFNJqSA==", "fe8cb99e-9f94-4e0a-9ca5-e9260e55cd5d" });
        }
    }
}
