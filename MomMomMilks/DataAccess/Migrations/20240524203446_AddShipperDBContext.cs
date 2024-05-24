using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddShipperDBContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Shipper_ShipperId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipper_AspNetUsers_AppUserId",
                table: "Shipper");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shipper",
                table: "Shipper");

            migrationBuilder.RenameTable(
                name: "Shipper",
                newName: "Shippers");

            migrationBuilder.RenameIndex(
                name: "IX_Shipper_AppUserId",
                table: "Shippers",
                newName: "IX_Shippers_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shippers",
                table: "Shippers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Shippers_ShipperId",
                table: "Schedules",
                column: "ShipperId",
                principalTable: "Shippers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shippers_AspNetUsers_AppUserId",
                table: "Shippers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Shippers_ShipperId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Shippers_AspNetUsers_AppUserId",
                table: "Shippers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shippers",
                table: "Shippers");

            migrationBuilder.RenameTable(
                name: "Shippers",
                newName: "Shipper");

            migrationBuilder.RenameIndex(
                name: "IX_Shippers_AppUserId",
                table: "Shipper",
                newName: "IX_Shipper_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shipper",
                table: "Shipper",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Shipper_ShipperId",
                table: "Schedules",
                column: "ShipperId",
                principalTable: "Shipper",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipper_AspNetUsers_AppUserId",
                table: "Shipper",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
