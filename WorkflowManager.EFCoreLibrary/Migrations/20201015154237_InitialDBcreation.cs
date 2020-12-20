using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkflowManager.EFCoreLibrary.Migrations
{
    public partial class InitialDBcreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(maxLength: 256, nullable: false),
                    Street = table.Column<string>(maxLength: 256, nullable: false),
                    AddressBuildingNum = table.Column<string>(maxLength: 256, nullable: true),
                    AddressAdditional = table.Column<string>(maxLength: 256, nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    ResidentSign = table.Column<byte[]>(nullable: true),
                    MountedHeatMetersAmount = table.Column<int>(nullable: true),
                    DisposedHeatMetersAmount = table.Column<int>(nullable: true),
                    LeftHeatMetersAmount = table.Column<int>(nullable: true),
                    HeatMetersRemarks = table.Column<string>(maxLength: 2048, nullable: true),
                    MountedWaterMetersAmount = table.Column<int>(nullable: true),
                    DisposedWaterMetersAmount = table.Column<int>(nullable: true),
                    LeftWaterMetersAmount = table.Column<int>(nullable: true),
                    WaterMetersRemarks = table.Column<string>(maxLength: 2048, nullable: true),
                    MountedCostMetersAmount = table.Column<int>(nullable: true),
                    DisposedCostMetersAmount = table.Column<int>(nullable: true),
                    LeftCostMetersAmount = table.Column<int>(nullable: true),
                    CostMetersRemarks = table.Column<string>(maxLength: 2048, nullable: true),
                    MountedOrReplacedFittingsAmount = table.Column<int>(nullable: true),
                    MountedOrReplacedReductionsAmount = table.Column<int>(nullable: true),
                    MountedOrReplacedValvesAmount = table.Column<int>(nullable: true),
                    MountedCheckValvesAmount = table.Column<int>(nullable: true),
                    ReplacedTeesAmount = table.Column<int>(nullable: true),
                    SealedWaterMetersAmount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 256, nullable: true),
                    LastName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 12, nullable: true),
                    Lock = table.Column<bool>(nullable: false),
                    BuildingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CostMeters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdBuilding = table.Column<int>(nullable: false),
                    Measurement = table.Column<float>(nullable: true),
                    Group = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    DemountedPKONum = table.Column<string>(maxLength: 256, nullable: true),
                    MountedPKONum = table.Column<string>(maxLength: 256, nullable: true),
                    CurrMeasurement = table.Column<float>(nullable: true),
                    CycleEndMeasurement = table.Column<float>(nullable: true),
                    Plate = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostMeters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostMeters_Buildings_IdBuilding",
                        column: x => x.IdBuilding,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeatMeters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdBuilding = table.Column<int>(nullable: false),
                    Measurement = table.Column<float>(nullable: true),
                    DemountedHeatMeterNum = table.Column<string>(maxLength: 256, nullable: true),
                    MountedHeatMeterNum = table.Column<string>(maxLength: 256, nullable: true),
                    DemountState = table.Column<string>(maxLength: 256, nullable: true),
                    MountState = table.Column<string>(maxLength: 256, nullable: true),
                    SealNum = table.Column<string>(maxLength: 256, nullable: true),
                    SensorSealNum = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeatMeters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeatMeters_Buildings_IdBuilding",
                        column: x => x.IdBuilding,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdBuilding = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Done = table.Column<bool>(nullable: false),
                    DoneDate = table.Column<DateTime>(nullable: false),
                    PredictedDoneDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Buildings_IdBuilding",
                        column: x => x.IdBuilding,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaterMeters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdBuilding = table.Column<int>(nullable: false),
                    Measurement = table.Column<float>(nullable: true),
                    DemountedWaterMeterNum = table.Column<string>(maxLength: 256, nullable: true),
                    MountedWaterMeterNum = table.Column<string>(maxLength: 256, nullable: true),
                    DemountState = table.Column<string>(maxLength: 256, nullable: true),
                    MountState = table.Column<string>(maxLength: 256, nullable: true),
                    SealNum = table.Column<string>(maxLength: 256, nullable: true),
                    CheckValve = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterMeters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterMeters_Buildings_IdBuilding",
                        column: x => x.IdBuilding,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserJob",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    JobId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJob", x => new { x.UserId, x.JobId });
                    table.ForeignKey(
                        name: "FK_UserJob_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserJob_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BuildingId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "Lock", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "e49015be-c88a-4f31-8b86-36342caed314", 0, null, "8cd87ab3-dadb-4992-9b7b-b55d447245c8", null, false, "Antoni", "Tyl", false, false, null, null, null, null, null, false, "503f663a-85d4-4124-9e55-1605ab0ec6dc", false, null },
                    { "2e60f867-7a47-474a-8d3d-3b9bd509c814", 0, null, "ab99b1fd-16bb-42b1-9a86-e87d37a5f7c3", null, false, "Maciej", "Waszalski", false, false, null, null, null, null, null, false, "39cfc5ba-bdbb-4294-9693-3c87358f394d", false, null }
                });

            migrationBuilder.InsertData(
                table: "Buildings",
                columns: new[] { "Id", "AddressAdditional", "AddressBuildingNum", "City", "CostMetersRemarks", "Date", "DisposedCostMetersAmount", "DisposedHeatMetersAmount", "DisposedWaterMetersAmount", "HeatMetersRemarks", "LeftCostMetersAmount", "LeftHeatMetersAmount", "LeftWaterMetersAmount", "MountedCheckValvesAmount", "MountedCostMetersAmount", "MountedHeatMetersAmount", "MountedOrReplacedFittingsAmount", "MountedOrReplacedReductionsAmount", "MountedOrReplacedValvesAmount", "MountedWaterMetersAmount", "ReplacedTeesAmount", "ResidentSign", "SealedWaterMetersAmount", "Street", "WaterMetersRemarks" },
                values: new object[,]
                {
                    { 1, "22", "23A", "Katowice", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "Polańska", null },
                    { 2, null, "5A", "Kraków", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "Ruczaj", null }
                });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "Id", "Deleted", "Description", "Done", "DoneDate", "Duration", "IdBuilding", "Name", "Order", "PredictedDoneDate" },
                values: new object[] { 1, false, "job1Description", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 32, 46, 0), 1, "job1Name", 1, new DateTime(2020, 10, 15, 21, 42, 36, 424, DateTimeKind.Local).AddTicks(4723) });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "Id", "Deleted", "Description", "Done", "DoneDate", "Duration", "IdBuilding", "Name", "Order", "PredictedDoneDate" },
                values: new object[] { 2, false, "job2Description", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 22, 56, 0), 2, "job2Name", 2, new DateTime(2020, 10, 15, 23, 42, 36, 429, DateTimeKind.Local).AddTicks(5641) });

            migrationBuilder.InsertData(
                table: "UserJob",
                columns: new[] { "UserId", "JobId" },
                values: new object[] { "2e60f867-7a47-474a-8d3d-3b9bd509c814", 1 });

            migrationBuilder.InsertData(
                table: "UserJob",
                columns: new[] { "UserId", "JobId" },
                values: new object[] { "e49015be-c88a-4f31-8b86-36342caed314", 2 });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BuildingId",
                table: "AspNetUsers",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CostMeters_IdBuilding",
                table: "CostMeters",
                column: "IdBuilding");

            migrationBuilder.CreateIndex(
                name: "IX_HeatMeters_IdBuilding",
                table: "HeatMeters",
                column: "IdBuilding");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_IdBuilding",
                table: "Jobs",
                column: "IdBuilding");

            migrationBuilder.CreateIndex(
                name: "IX_UserJob_JobId",
                table: "UserJob",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterMeters_IdBuilding",
                table: "WaterMeters",
                column: "IdBuilding");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CostMeters");

            migrationBuilder.DropTable(
                name: "HeatMeters");

            migrationBuilder.DropTable(
                name: "UserJob");

            migrationBuilder.DropTable(
                name: "WaterMeters");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Buildings");
        }
    }
}
