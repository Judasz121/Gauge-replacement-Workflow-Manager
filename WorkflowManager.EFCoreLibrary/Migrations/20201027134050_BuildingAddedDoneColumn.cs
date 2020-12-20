using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkflowManager.EFCoreLibrary.Migrations
{
    public partial class BuildingAddedDoneColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "Done",
                table: "Buildings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BuildingId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "Lock", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "5961d94d-0e6b-46ba-a8de-88cd4fcc7121", 0, null, "3db4ddf0-e7de-40fb-9c89-643a6fe3d5f5", null, false, "Antoni", "Tyl", false, false, null, null, null, null, null, false, "9550aed1-d73a-4c54-810e-a55335445f0f", false, null },
                    { "583f5944-ca2e-466d-bf28-961365042b5a", 0, null, "16776991-4032-4ecf-abc5-15f408cfb4e2", null, false, "Maciej", "Waszalski", false, false, null, null, null, null, null, false, "18147be4-f576-4c28-b144-20f5995a9709", false, null }
                });

            migrationBuilder.UpdateData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 1,
                column: "PredictedDoneDate",
                value: new DateTime(2020, 10, 27, 18, 40, 49, 625, DateTimeKind.Local).AddTicks(797));

            migrationBuilder.UpdateData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 2,
                column: "PredictedDoneDate",
                value: new DateTime(2020, 10, 27, 20, 40, 49, 631, DateTimeKind.Local).AddTicks(450));

            migrationBuilder.InsertData(
                table: "UserJob",
                columns: new[] { "UserId", "JobId" },
                values: new object[] { "5961d94d-0e6b-46ba-a8de-88cd4fcc7121", 2 });

            migrationBuilder.InsertData(
                table: "UserJob",
                columns: new[] { "UserId", "JobId" },
                values: new object[] { "583f5944-ca2e-466d-bf28-961365042b5a", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserJob",
                keyColumns: new[] { "UserId", "JobId" },
                keyValues: new object[] { "583f5944-ca2e-466d-bf28-961365042b5a", 1 });

            migrationBuilder.DeleteData(
                table: "UserJob",
                keyColumns: new[] { "UserId", "JobId" },
                keyValues: new object[] { "5961d94d-0e6b-46ba-a8de-88cd4fcc7121", 2 });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "583f5944-ca2e-466d-bf28-961365042b5a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5961d94d-0e6b-46ba-a8de-88cd4fcc7121");

            migrationBuilder.DropColumn(
                name: "Done",
                table: "Buildings");

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
                column: "PredictedDoneDate",
                value: new DateTime(2020, 10, 20, 18, 21, 30, 383, DateTimeKind.Local).AddTicks(1742));

            migrationBuilder.UpdateData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 2,
                column: "PredictedDoneDate",
                value: new DateTime(2020, 10, 20, 20, 21, 30, 388, DateTimeKind.Local).AddTicks(2662));

            migrationBuilder.InsertData(
                table: "UserJob",
                columns: new[] { "UserId", "JobId" },
                values: new object[] { "666fa140-78d6-40d3-94b1-373777768579", 2 });

            migrationBuilder.InsertData(
                table: "UserJob",
                columns: new[] { "UserId", "JobId" },
                values: new object[] { "3e87ccd2-e09d-4e2b-9f5c-b4c956e9955d", 1 });
        }
    }
}
