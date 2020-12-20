using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkflowManager.EFCoreLibrary.Migrations
{
    public partial class MetersPropertyNameChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostMeters_Buildings_IdBuilding",
                table: "CostMeters");

            migrationBuilder.DropForeignKey(
                name: "FK_HeatMeters_Buildings_IdBuilding",
                table: "HeatMeters");

            migrationBuilder.DropForeignKey(
                name: "FK_WaterMeters_Buildings_IdBuilding",
                table: "WaterMeters");

            migrationBuilder.DropIndex(
                name: "IX_WaterMeters_IdBuilding",
                table: "WaterMeters");

            migrationBuilder.DropIndex(
                name: "IX_HeatMeters_IdBuilding",
                table: "HeatMeters");

            migrationBuilder.DropIndex(
                name: "IX_CostMeters_IdBuilding",
                table: "CostMeters");

            migrationBuilder.DropColumn(
                name: "IdBuilding",
                table: "WaterMeters");

            migrationBuilder.DropColumn(
                name: "IdBuilding",
                table: "HeatMeters");

            migrationBuilder.DropColumn(
                name: "IdBuilding",
                table: "CostMeters");

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "WaterMeters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "HeatMeters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "CostMeters",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_WaterMeters_BuildingId",
                table: "WaterMeters",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_HeatMeters_BuildingId",
                table: "HeatMeters",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_CostMeters_BuildingId",
                table: "CostMeters",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_CostMeters_Buildings_BuildingId",
                table: "CostMeters",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HeatMeters_Buildings_BuildingId",
                table: "HeatMeters",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WaterMeters_Buildings_BuildingId",
                table: "WaterMeters",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostMeters_Buildings_BuildingId",
                table: "CostMeters");

            migrationBuilder.DropForeignKey(
                name: "FK_HeatMeters_Buildings_BuildingId",
                table: "HeatMeters");

            migrationBuilder.DropForeignKey(
                name: "FK_WaterMeters_Buildings_BuildingId",
                table: "WaterMeters");

            migrationBuilder.DropIndex(
                name: "IX_WaterMeters_BuildingId",
                table: "WaterMeters");

            migrationBuilder.DropIndex(
                name: "IX_HeatMeters_BuildingId",
                table: "HeatMeters");

            migrationBuilder.DropIndex(
                name: "IX_CostMeters_BuildingId",
                table: "CostMeters");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "WaterMeters");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "HeatMeters");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "CostMeters");

            migrationBuilder.AddColumn<int>(
                name: "IdBuilding",
                table: "WaterMeters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdBuilding",
                table: "HeatMeters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdBuilding",
                table: "CostMeters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "9898364a-d32a-492c-873d-1ec8ea77df42");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "manager",
                column: "ConcurrencyStamp",
                value: "eaf0a4e3-07da-4a45-8e2b-dab4c948bad7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "5e046d6f-ff08-488e-9a45-573280e7462a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "655efe59-e0da-499f-b60b-13576646c483", "AQAAAAEAACcQAAAAEP9WYvA9y8Ehulb37Jc6iKiqbVWNB/nzgCVeiNT4XOgRTMMH7GStSgxQFH4s9VF1Sw==", "14b42bbf-9c67-464f-bfb8-be7eb3813117" });

            migrationBuilder.CreateIndex(
                name: "IX_WaterMeters_IdBuilding",
                table: "WaterMeters",
                column: "IdBuilding");

            migrationBuilder.CreateIndex(
                name: "IX_HeatMeters_IdBuilding",
                table: "HeatMeters",
                column: "IdBuilding");

            migrationBuilder.CreateIndex(
                name: "IX_CostMeters_IdBuilding",
                table: "CostMeters",
                column: "IdBuilding");

            migrationBuilder.AddForeignKey(
                name: "FK_CostMeters_Buildings_IdBuilding",
                table: "CostMeters",
                column: "IdBuilding",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HeatMeters_Buildings_IdBuilding",
                table: "HeatMeters",
                column: "IdBuilding",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WaterMeters_Buildings_IdBuilding",
                table: "WaterMeters",
                column: "IdBuilding",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
