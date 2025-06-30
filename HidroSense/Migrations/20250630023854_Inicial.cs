using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HidroSense.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SistemasPurificacion",
                columns: table => new
                {
                    IdSistema = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreSistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SistemasPurificacion", x => x.IdSistema);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "FuentesAgua",
                columns: table => new
                {
                    IdFuente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    NombreFuente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoFuente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitud = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Altitud = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuentesAgua", x => x.IdFuente);
                    table.ForeignKey(
                        name: "FK_FuentesAgua_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    IdAlerta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFuente = table.Column<int>(type: "int", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoAlerta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.IdAlerta);
                    table.ForeignKey(
                        name: "FK_Alertas_FuentesAgua_IdFuente",
                        column: x => x.IdFuente,
                        principalTable: "FuentesAgua",
                        principalColumn: "IdFuente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mediciones",
                columns: table => new
                {
                    IdMedicion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFuente = table.Column<int>(type: "int", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ph = table.Column<float>(type: "real", nullable: true),
                    NivelTurbidez = table.Column<float>(type: "real", nullable: true),
                    Temperatura = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mediciones", x => x.IdMedicion);
                    table.ForeignKey(
                        name: "FK_Mediciones_FuentesAgua_IdFuente",
                        column: x => x.IdFuente,
                        principalTable: "FuentesAgua",
                        principalColumn: "IdFuente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TratamientosAplicados",
                columns: table => new
                {
                    IdTratamiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFuente = table.Column<int>(type: "int", nullable: false),
                    IdSistema = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TratamientosAplicados", x => x.IdTratamiento);
                    table.ForeignKey(
                        name: "FK_TratamientosAplicados_FuentesAgua_IdFuente",
                        column: x => x.IdFuente,
                        principalTable: "FuentesAgua",
                        principalColumn: "IdFuente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TratamientosAplicados_SistemasPurificacion_IdSistema",
                        column: x => x.IdSistema,
                        principalTable: "SistemasPurificacion",
                        principalColumn: "IdSistema",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResultadosTratamiento",
                columns: table => new
                {
                    IdResultado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTratamiento = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhFinal = table.Column<float>(type: "real", nullable: true),
                    TurbidezFinal = table.Column<float>(type: "real", nullable: true),
                    TemperaturaFinal = table.Column<float>(type: "real", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosTratamiento", x => x.IdResultado);
                    table.ForeignKey(
                        name: "FK_ResultadosTratamiento_TratamientosAplicados_IdTratamiento",
                        column: x => x.IdTratamiento,
                        principalTable: "TratamientosAplicados",
                        principalColumn: "IdTratamiento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_IdFuente",
                table: "Alertas",
                column: "IdFuente");

            migrationBuilder.CreateIndex(
                name: "IX_FuentesAgua_IdUsuario",
                table: "FuentesAgua",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Mediciones_IdFuente",
                table: "Mediciones",
                column: "IdFuente");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosTratamiento_IdTratamiento",
                table: "ResultadosTratamiento",
                column: "IdTratamiento");

            migrationBuilder.CreateIndex(
                name: "IX_TratamientosAplicados_IdFuente",
                table: "TratamientosAplicados",
                column: "IdFuente");

            migrationBuilder.CreateIndex(
                name: "IX_TratamientosAplicados_IdSistema",
                table: "TratamientosAplicados",
                column: "IdSistema");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "Mediciones");

            migrationBuilder.DropTable(
                name: "ResultadosTratamiento");

            migrationBuilder.DropTable(
                name: "TratamientosAplicados");

            migrationBuilder.DropTable(
                name: "FuentesAgua");

            migrationBuilder.DropTable(
                name: "SistemasPurificacion");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
