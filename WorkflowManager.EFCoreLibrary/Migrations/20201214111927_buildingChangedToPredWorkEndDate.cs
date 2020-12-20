using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkflowManager.EFCoreLibrary.Migrations
{
    public partial class buildingChangedToPredWorkEndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PredictedWorkTime",
                table: "Buildings");

            migrationBuilder.AddColumn<DateTime>(
                name: "PredictedWorkEndDate",
                table: "Buildings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "9910d083-251d-4625-a88e-ad71f8c868a4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "manager",
                column: "ConcurrencyStamp",
                value: "28852846-d717-4d62-b39b-9f632713107c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "9c894cb1-6495-4961-9ae9-cbf0860f5510");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "022800df-d061-4809-9466-fb69a7d63b79", "AQAAAAEAACcQAAAAEH30z7g3aPAdZPloaEtLzGwarzNWQ7iLEKjHmUZSTQWE4fhXKracO/Hjo5Nq6RgZog==", "08b61b90-a746-4200-a639-aa685cb7cf6a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PredictedWorkEndDate",
                table: "Buildings");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PredictedWorkTime",
                table: "Buildings",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

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
    }
}
