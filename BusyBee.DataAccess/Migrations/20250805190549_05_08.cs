using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusyBee.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _05_08 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EditedById",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EditedById",
                table: "Orders",
                column: "EditedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_EditedById",
                table: "Orders",
                column: "EditedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_EditedById",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_EditedById",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "EditedById",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");
        }
    }
}
