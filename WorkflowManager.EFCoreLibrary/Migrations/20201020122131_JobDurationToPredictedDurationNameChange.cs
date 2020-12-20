using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkflowManager.EFCoreLibrary.Migrations
{
    public partial class JobDurationToPredictedDurationNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserJob",
                keyColumns: new[] { "UserId", "JobId" },
                keyValues: new object[] { "2e60f867-7a47-474a-8d3d-3b9bd509c814", 1 });

            migrationBuilder.DeleteData(
                table: "UserJob",
                keyColumns: new[] { "UserId", "JobId" },
                keyValues: new object[] { "e49015be-c88a-4f31-8b86-36342caed314", 2 });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e60f867-7a47-474a-8d3d-3b9bd509c814");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e49015be-c88a-4f31-8b86-36342caed314");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PredictedDuration",
                table: "Jobs",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BuildingId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "Lock", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "666fa140-78d6-40d3-94b1-373777768579", 0, null, "642bd2bd-02aa-4ac6-afbf-bab682374dfd", null, false, "Antoni", "Tyl", false, false, null, null, null, null, null, false, "a42c4e71-8320-41a7-82c1-8856a54353a2", false, null },
                    { "3e87ccd2-e09d-4e2b-9f5c-b4c956e9955d", 0, null, "1575f3ac-b8e9-4d2c-bc59-26f7f9522477", null, false, "Maciej", "Waszalski", false, false, null, null, null, null, null, false, "7c68c062-cc22-4a14-b3c0-08ffb93d0fb4", false, null }
                });

            migrationBuilder.UpdateData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PredictedDoneDate", "PredictedDuration" },
                values: new object[] { new DateTime(2020, 10, 20, 18, 21, 30, 383, DateTimeKind.Local).AddTicks(1742), new TimeSpan(0, 1, 32, 46, 0) });

            migrationBuilder.UpdateData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PredictedDoneDate", "PredictedDuration" },
                values: new object[] { new DateTime(2020, 10, 20, 20, 21, 30, 388, DateTimeKind.Local).AddTicks(2662), new TimeSpan(0, 2, 22, 56, 0) });

            migrationBuilder.InsertData(
                table: "UserJob",
                columns: new[] { "UserId", "JobId" },
                values: new object[] { "666fa140-78d6-40d3-94b1-373777768579", 2 });

            migrationBuilder.InsertData(
                table: "UserJob",
                columns: new[] { "UserId", "JobId" },
                values: new object[] { "3e87ccd2-e09d-4e2b-9f5c-b4c956e9955d", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserJob",
                keyColumns: new[] { "UserId", "JobId" },
                keyValues: new object[] { "3e87ccd2-e09d-4e2b-9f5c-b4c956e9955d", 1 });

            migrationBuilder.DeleteData(
                table: "UserJob",
                keyColumns: new[] { "UserId", "JobId" },
                keyValues: new object[] { "666fa140-78d6-40d3-94b1-373777768579", 2 });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3e87ccd2-e09d-4e2b-9f5c-b4c956e9955d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "666fa140-78d6-40d3-94b1-373777768579");

            migrationBuilder.DropColumn(
                name: "PredictedDuration",
                table: "Jobs");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Jobs",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BuildingId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "Lock", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "e49015be-c88a-4f31-8b86-36342caed314", 0, null, "8cd87ab3-dadb-4992-9b7b-b55d447245c8", null, false, "Antoni", "Tyl", false, false, null, null, null, null, null, false, "503f663a-85d4-4124-9e55-1605ab0ec6dc", false, null },
                    { "2e60f867-7a47-474a-8d3d-3b9bd509c814", 0, null, "ab99b1fd-16bb-42b1-9a86-e87d37a5f7c3", null, false, "Maciej", "Waszalski", false, false, null, null, null, null, null, false, "39cfc5ba-bdbb-4294-9693-3c87358f394d", false, null }
                });

            migrationBuilder.UpdateData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Duration", "PredictedDoneDate" },
                values: new object[] { new TimeSpan(0, 1, 32, 46, 0), new DateTime(2020, 10, 15, 21, 42, 36, 424, DateTimeKind.Local).AddTicks(4723) });

            migrationBuilder.UpdateData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Duration", "PredictedDoneDate" },
                values: new object[] { new TimeSpan(0, 2, 22, 56, 0), new DateTime(2020, 10, 15, 23, 42, 36, 429, DateTimeKind.Local).AddTicks(5641) });

            migrationBuilder.InsertData(
                table: "UserJob",
                columns: new[] { "UserId", "JobId" },
                values: new object[] { "e49015be-c88a-4f31-8b86-36342caed314", 2 });

            migrationBuilder.InsertData(
                table: "UserJob",
                columns: new[] { "UserId", "JobId" },
                values: new object[] { "2e60f867-7a47-474a-8d3d-3b9bd509c814", 1 });
        }
    }
}
