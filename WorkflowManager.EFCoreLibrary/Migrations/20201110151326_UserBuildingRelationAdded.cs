using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkflowManager.EFCoreLibrary.Migrations
{
    public partial class UserBuildingRelationAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Buildings_BuildingId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BuildingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateWorkEnd",
                table: "Buildings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateWorkStart",
                table: "Buildings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PredictedWorkTime",
                table: "Buildings",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "UserBuilding",
                columns: table => new
                {
                    BuildingId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBuilding", x => new { x.UserId, x.BuildingId });
                    table.ForeignKey(
                        name: "FK_UserBuilding_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBuilding_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "15a79ac5-e06b-43c0-9722-7b6f43d80a92");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "manager",
                column: "ConcurrencyStamp",
                value: "3bf497e6-a939-4571-b41c-e48bf36ffff5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "b202cf2c-4146-4559-b70e-7fc5866716c2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6c0971f3-0ba0-437a-bb51-600b534f0991", "AQAAAAEAACcQAAAAECmAXffvHheVs9nBMkKQ5SJ3fxRAbQwYlsrnkST8No/vOOeDF9UcDQKhGioWMd3iPw==", "2e7a4bae-0087-4c42-9152-abe87dd54b96" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBuilding_BuildingId",
                table: "UserBuilding",
                column: "BuildingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBuilding");

            migrationBuilder.DropColumn(
                name: "DateWorkEnd",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "DateWorkStart",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "PredictedWorkTime",
                table: "Buildings");

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "3245bae1-84e3-4b19-9f98-c6df51bb5e56");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "manager",
                column: "ConcurrencyStamp",
                value: "81b885d0-6d68-4559-8300-3606ae6eb8ea");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "9d4436e9-cc6d-4f29-83b3-b6f1857dba98");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c27b28bc-3a93-40e7-973b-e7b7ad9a48c0", "AQAAAAEAACcQAAAAEIgHpomH+aQM4ewCjcgjoxnQ5GxPHrUu9KgWFysP6hezLtgAXTaCBuFZd2F5N3BbgA==", "1a4e30de-6635-4adc-a653-b5415238ae99" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BuildingId",
                table: "AspNetUsers",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Buildings_BuildingId",
                table: "AspNetUsers",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
