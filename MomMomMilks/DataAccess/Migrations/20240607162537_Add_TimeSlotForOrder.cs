using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Add_TimeSlotForOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Orders_OrderId",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TimeSlot",
                table: "Schedules");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transactions");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_OrderId",
                table: "Transactions",
                newName: "IX_Transactions_OrderId");

            migrationBuilder.AddColumn<int>(
                name: "TimeSlotId",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeSlotId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TimeSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TimeSlotId",
                table: "Schedules",
                column: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TimeSlotId",
                table: "Orders",
                column: "TimeSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_TimeSlots_TimeSlotId",
                table: "Orders",
                column: "TimeSlotId",
                principalTable: "TimeSlots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_TimeSlots_TimeSlotId",
                table: "Schedules",
                column: "TimeSlotId",
                principalTable: "TimeSlots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Orders_OrderId",
                table: "Transactions",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_TimeSlots_TimeSlotId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_TimeSlots_TimeSlotId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Orders_OrderId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "TimeSlots");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_TimeSlotId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TimeSlotId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TimeSlotId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "TimeSlotId",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transaction");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_OrderId",
                table: "Transaction",
                newName: "IX_Transaction_OrderId");

            migrationBuilder.AddColumn<string>(
                name: "TimeSlot",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Orders_OrderId",
                table: "Transaction",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
