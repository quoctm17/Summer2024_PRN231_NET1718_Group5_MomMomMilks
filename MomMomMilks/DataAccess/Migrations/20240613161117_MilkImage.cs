using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MilkImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Milks");

            migrationBuilder.CreateTable(
                name: "MilkImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isMain = table.Column<bool>(type: "bit", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MilkId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilkImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilkImage_Milks_MilkId",
                        column: x => x.MilkId,
                        principalTable: "Milks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MilkImage_MilkId",
                table: "MilkImage",
                column: "MilkId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MilkImage");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Milks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
