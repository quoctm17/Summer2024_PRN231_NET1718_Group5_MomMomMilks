using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Add_ShipperDistrictResponsible : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "Shippers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shippers_DistrictId",
                table: "Shippers",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shippers_Districts_DistrictId",
                table: "Shippers",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shippers_Districts_DistrictId",
                table: "Shippers");

            migrationBuilder.DropIndex(
                name: "IX_Shippers_DistrictId",
                table: "Shippers");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Shippers");
        }
    }
}
