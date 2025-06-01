using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SweetNela.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePagoYOrden : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_pago_AspNetUsers_UserId",
                table: "t_pago");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "t_order",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "t_pago",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_order_UserId",
                table: "t_order",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_t_order_AspNetUsers_UserId",
                table: "t_order",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_t_pago_AspNetUsers_UserId",
                table: "t_pago",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_order_AspNetUsers_UserId",
                table: "t_order");

            migrationBuilder.DropForeignKey(
                name: "FK_t_pago_AspNetUsers_UserId",
                table: "t_pago");

            migrationBuilder.DropIndex(
                name: "IX_t_order_UserId",
                table: "t_order");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "t_order",
                newName: "UserName");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "t_pago",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_t_pago_AspNetUsers_UserId",
                table: "t_pago",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
