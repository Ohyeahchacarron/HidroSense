using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HidroSense.Migrations
{
    /// <inheritdoc />
    public partial class nueva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    IdProveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProveedor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreContacto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.IdProveedor);
                });

            migrationBuilder.CreateTable(
                name: "ComponentesSistema",
                columns: table => new
                {
                    IdComponente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSistema = table.Column<int>(type: "int", nullable: false),
                    SistemaPurificacionIdSistema = table.Column<int>(type: "int", nullable: false),
                    NombreComponente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<double>(type: "float", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    IdProveedor = table.Column<int>(type: "int", nullable: false),
                    ProveedorIdProveedor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentesSistema", x => x.IdComponente);
                    table.ForeignKey(
                        name: "FK_ComponentesSistema_Proveedores_ProveedorIdProveedor",
                        column: x => x.ProveedorIdProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "IdProveedor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComponentesSistema_SistemasPurificacion_SistemaPurificacionIdSistema",
                        column: x => x.SistemaPurificacionIdSistema,
                        principalTable: "SistemasPurificacion",
                        principalColumn: "IdSistema",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentesSistema_ProveedorIdProveedor",
                table: "ComponentesSistema",
                column: "ProveedorIdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentesSistema_SistemaPurificacionIdSistema",
                table: "ComponentesSistema",
                column: "SistemaPurificacionIdSistema");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentesSistema");

            migrationBuilder.DropTable(
                name: "Proveedores");
        }
    }
}
