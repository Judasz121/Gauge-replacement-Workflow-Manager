using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkflowManager.EFCoreLibrary.Migrations
{
    public partial class AddedJobDateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Jobs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Jobs");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "f56c898e-a21e-4cd7-b426-1e7c0c12e5b4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "manager",
                column: "ConcurrencyStamp",
                value: "d58fbf23-e0d5-458d-b85b-8fa3deb5d2a2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "f8fd76f9-090d-4537-b2d7-83044f85702d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "201da232-03db-470b-85e4-711da31422c3", "AQAAAAEAACcQAAAAEOIZg9Y7qHD4rAb8QnMiA+kQSVdp4KRW3mofCMVmMRVW0EAxXJqXOkWHTTq1E7IN2A==", "a7627b7f-e724-4fa9-bee1-ce52f7f90093" });
        }
    }
}
