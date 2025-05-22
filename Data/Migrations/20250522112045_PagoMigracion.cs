using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SweetNela.Data.Migrations
{
    /// <inheritdoc />
    public partial class PagoMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_pago",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NombreTarjeta = table.Column<string>(type: "text", nullable: true),
                    NumeroTarjeta = table.Column<string>(type: "text", nullable: true),
                    MontoTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_pago", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t_order",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PagoId = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_order_t_pago_PagoId",
                        column: x => x.PagoId,
                        principalTable: "t_pago",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "t_order_detail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoId = table.Column<int>(type: "integer", nullable: true),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    Precio = table.Column<decimal>(type: "numeric", nullable: false),
                    OrdenId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_order_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_order_detail_t_order_OrdenId",
                        column: x => x.OrdenId,
                        principalTable: "t_order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_t_order_detail_t_producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "t_producto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_order_PagoId",
                table: "t_order",
                column: "PagoId");

            migrationBuilder.CreateIndex(
                name: "IX_t_order_detail_OrdenId",
                table: "t_order_detail",
                column: "OrdenId");

            migrationBuilder.CreateIndex(
                name: "IX_t_order_detail_ProductoId",
                table: "t_order_detail",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_order_detail");

            migrationBuilder.DropTable(
                name: "t_order");

            migrationBuilder.DropTable(
                name: "t_pago");
        }
    }
}
