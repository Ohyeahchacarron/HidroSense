using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HidroSense.Migrations
{
    /// <inheritdoc />
    public partial class Componentes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentesSistema_Proveedores_ProveedorIdProveedor",
                table: "ComponentesSistema");

            migrationBuilder.DropForeignKey(
                name: "FK_ComponentesSistema_SistemasPurificacion_SistemaPurificacionIdSistema",
                table: "ComponentesSistema");

            migrationBuilder.DropIndex(
                name: "IX_ComponentesSistema_ProveedorIdProveedor",
                table: "ComponentesSistema");

            migrationBuilder.DropIndex(
                name: "IX_ComponentesSistema_SistemaPurificacionIdSistema",
                table: "ComponentesSistema");

            migrationBuilder.DropColumn(
                name: "ProveedorIdProveedor",
                table: "ComponentesSistema");

            migrationBuilder.DropColumn(
                name: "SistemaPurificacionIdSistema",
                table: "ComponentesSistema");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentesSistema_IdProveedor",
                table: "ComponentesSistema",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentesSistema_IdSistema",
                table: "ComponentesSistema",
                column: "IdSistema");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentesSistema_Proveedores_IdProveedor",
                table: "ComponentesSistema",
                column: "IdProveedor",
                principalTable: "Proveedores",
                principalColumn: "IdProveedor",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentesSistema_SistemasPurificacion_IdSistema",
                table: "ComponentesSistema",
                column: "IdSistema",
                principalTable: "SistemasPurificacion",
                principalColumn: "IdSistema",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentesSistema_Proveedores_IdProveedor",
                table: "ComponentesSistema");

            migrationBuilder.DropForeignKey(
                name: "FK_ComponentesSistema_SistemasPurificacion_IdSistema",
                table: "ComponentesSistema");

            migrationBuilder.DropIndex(
                name: "IX_ComponentesSistema_IdProveedor",
                table: "ComponentesSistema");

            migrationBuilder.DropIndex(
                name: "IX_ComponentesSistema_IdSistema",
                table: "ComponentesSistema");

            migrationBuilder.AddColumn<int>(
                name: "ProveedorIdProveedor",
                table: "ComponentesSistema",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SistemaPurificacionIdSistema",
                table: "ComponentesSistema",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ComponentesSistema_ProveedorIdProveedor",
                table: "ComponentesSistema",
                column: "ProveedorIdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentesSistema_SistemaPurificacionIdSistema",
                table: "ComponentesSistema",
                column: "SistemaPurificacionIdSistema");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentesSistema_Proveedores_ProveedorIdProveedor",
                table: "ComponentesSistema",
                column: "ProveedorIdProveedor",
                principalTable: "Proveedores",
                principalColumn: "IdProveedor",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentesSistema_SistemasPurificacion_SistemaPurificacionIdSistema",
                table: "ComponentesSistema",
                column: "SistemaPurificacionIdSistema",
                principalTable: "SistemasPurificacion",
                principalColumn: "IdSistema",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
