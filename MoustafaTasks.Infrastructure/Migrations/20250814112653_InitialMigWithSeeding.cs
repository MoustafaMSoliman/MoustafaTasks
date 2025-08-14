using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoustafaTasks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigWithSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "DeviceType", "IsActived", "IsRemoved", "NameEnglish" },
                values: new object[,]
                {
                    { 1, "Camera", true, false, "Device1" },
                    { 2, "Another", false, false, "Device2" },
                    { 3, "Camera", false, true, "Device3" }
                });

            migrationBuilder.InsertData(
                table: "SecUsers",
                columns: new[] { "UserId", "IsActived", "IsRemoved", "UserName", "UserTypeId" },
                values: new object[,]
                {
                    { 1, true, false, "username1", "Person" },
                    { 2, false, false, "username2", "ServiceAccount" },
                    { 3, false, true, "username3", "Person" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SecUsers",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SecUsers",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SecUsers",
                keyColumn: "UserId",
                keyValue: 3);
        }
    }
}
