using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkflowManager.EFCoreLibrary.Migrations
{
    public partial class BuildingJointsNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplacedTeesAmount",
                table: "Buildings");

            migrationBuilder.AddColumn<int>(
                name: "ReplacedJointsAmount",
                table: "Buildings",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "cf043c5d-7838-4019-b2c5-743e7da5a3b4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "manager",
                column: "ConcurrencyStamp",
                value: "e341f7b1-0c18-4c46-94ae-4c16338ec4b9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "57e17e54-aa30-4b80-910e-033e47065057");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8d9dcd72-090c-4696-875f-90e62c1c4f5f", "AQAAAAEAACcQAAAAEOwm3hzzPPLJyVRJxW/cuTljDzlcTaXCXDETznIR22UE4Z7nV87bMZVYn8vWcle28Q==", "65cd65be-266b-4fc9-aa0b-0fbba4bcca75" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplacedJointsAmount",
                table: "Buildings");

            migrationBuilder.AddColumn<int>(
                name: "ReplacedTeesAmount",
                table: "Buildings",
                type: "int",
                nullable: true);

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
    }
}
