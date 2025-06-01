using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SweetNela.Data.Migrations
{
    /// <inheritdoc />
    public partial class Cambio_t_pago_y_t_PreOrden : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_order_t_pago_PagoId",
                table: "t_order");

            migrationBuilder.DropIndex(
                name: "IX_t_order_PagoId",
                table: "t_order");

            migrationBuilder.DropColumn(
                name: "PagoId",
                table: "t_order");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "t_pago",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "NumeroTarjeta",
                table: "t_pago",
                newName: "PayPalPaymentId");

            migrationBuilder.RenameColumn(
                name: "NombreTarjeta",
                table: "t_pago",
                newName: "PayPalPayerId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "t_preorden",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "t_pago",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_pago_OrderId",
                table: "t_pago",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_pago_UserId",
                table: "t_pago",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_t_pago_AspNetUsers_UserId",
                table: "t_pago",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_t_pago_t_order_OrderId",
                table: "t_pago",
                column: "OrderId",
                principalTable: "t_order",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_pago_AspNetUsers_UserId",
                table: "t_pago");

            migrationBuilder.DropForeignKey(
                name: "FK_t_pago_t_order_OrderId",
                table: "t_pago");

            migrationBuilder.DropIndex(
                name: "IX_t_pago_OrderId",
                table: "t_pago");

            migrationBuilder.DropIndex(
                name: "IX_t_pago_UserId",
                table: "t_pago");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "t_preorden");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "t_pago");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "t_pago",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "PayPalPaymentId",
                table: "t_pago",
                newName: "NumeroTarjeta");

            migrationBuilder.RenameColumn(
                name: "PayPalPayerId",
                table: "t_pago",
                newName: "NombreTarjeta");

            migrationBuilder.AddColumn<int>(
                name: "PagoId",
                table: "t_order",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_order_PagoId",
                table: "t_order",
                column: "PagoId");

            migrationBuilder.AddForeignKey(
                name: "FK_t_order_t_pago_PagoId",
                table: "t_order",
                column: "PagoId",
                principalTable: "t_pago",
                principalColumn: "Id");
        }
    }
}
