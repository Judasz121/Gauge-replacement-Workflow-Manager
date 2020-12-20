using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkflowManager.EFCoreLibrary.Migrations
{
    public partial class RemovedDevelopmentDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Buildings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Buildings",
                keyColumn: "Id",
                keyValue: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BuildingId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "Lock", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "5961d94d-0e6b-46ba-a8de-88cd4fcc7121", 0, null, "3db4ddf0-e7de-40fb-9c89-643a6fe3d5f5", null, false, "Antoni", "Tyl", false, false, null, null, null, null, null, false, "9550aed1-d73a-4c54-810e-a55335445f0f", false, null },
                    { "583f5944-ca2e-466d-bf28-961365042b5a", 0, null, "16776991-4032-4ecf-abc5-15f408cfb4e2", null, false, "Maciej", "Waszalski", false, false, null, null, null, null, null, false, "18147be4-f576-4c28-b144-20f5995a9709", false, null }
                });

            migrationBuilder.InsertData(
                table: "Buildings",
                columns: new[] { "Id", "AddressAdditional", "AddressBuildingNum", "City", "CostMetersRemarks", "Date", "DisposedCostMetersAmount", "DisposedHeatMetersAmount", "DisposedWaterMetersAmount", "Done", "HeatMetersRemarks", "LeftCostMetersAmount", "LeftHeatMetersAmount", "LeftWaterMetersAmount", "MountedCheckValvesAmount", "MountedCostMetersAmount", "MountedHeatMetersAmount", "MountedOrReplacedFittingsAmount", "MountedOrReplacedReductionsAmount", "MountedOrReplacedValvesAmount", "MountedWaterMetersAmount", "ReplacedTeesAmount", "ResidentSign", "SealedWaterMetersAmount", "Street", "WaterMetersRemarks" },
                values: new object[,]
                {
                    { 1, "22", "23A", "Katowice", null, null, null, null, null, false, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "Polańska", null },
                    { 2, null, "5A", "Kraków", null, null, null, null, null, false, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "Ruczaj", null }
                });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "Id", "Deleted", "Description", "Done", "DoneDate", "IdBuilding", "Name", "Order", "PredictedDoneDate", "PredictedDuration" },
                values: new object[] { 1, false, "job1Description", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "job1Name", 1, new DateTime(2020, 10, 27, 18, 40, 49, 625, DateTimeKind.Local).AddTicks(797), new TimeSpan(0, 1, 32, 46, 0) });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "Id", "Deleted", "Description", "Done", "DoneDate", "IdBuilding", "Name", "Order", "PredictedDoneDate", "PredictedDuration" },
                values: new object[] { 2, false, "job2Description", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "job2Name", 2, new DateTime(2020, 10, 27, 20, 40, 49, 631, DateTimeKind.Local).AddTicks(450), new TimeSpan(0, 2, 22, 56, 0) });

            migrationBuilder.InsertData(
                table: "UserJob",
                columns: new[] { "UserId", "JobId" },
                values: new object[] { "583f5944-ca2e-466d-bf28-961365042b5a", 1 });

            migrationBuilder.InsertData(
                table: "UserJob",
                columns: new[] { "UserId", "JobId" },
                values: new object[] { "5961d94d-0e6b-46ba-a8de-88cd4fcc7121", 2 });
        }
    }
}
