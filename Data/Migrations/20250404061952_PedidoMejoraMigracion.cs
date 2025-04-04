using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SweetNela.Data.Migrations
{
    /// <inheritdoc />
    public partial class PedidoMejoraMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.CreateTable(
                name: "t_PedidoMejora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SumaTotal = table.Column<double>(type: "REAL", nullable: false),
                    Sabor = table.Column<string>(type: "TEXT", nullable: true),
                    Tamaño = table.Column<string>(type: "TEXT", nullable: true),
                    Relleno = table.Column<string>(type: "TEXT", nullable: true),
                    Detalles = table.Column<string>(type: "TEXT", nullable: true),
                    Fecha = table.Column<string>(type: "TEXT", nullable: true),
                    Hora = table.Column<string>(type: "TEXT", nullable: true),
                    Lugar = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_PedidoMejora", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_PedidoMejora");
        }
    }
}
