using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HidroSense.Migrations
{
    /// <inheritdoc />
    public partial class revision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cotizaciones",
                columns: table => new
                {
                    IdCotizacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreContacto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SituacionDetallada = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cotizaciones", x => x.IdCotizacion);
                });

            migrationBuilder.CreateTable(
                name: "FuentesAgua",
                columns: table => new
                {
                    IdFuente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreFuente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoFuente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitud = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Altitud = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuentesAgua", x => x.IdFuente);
                });

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
                name: "SistemasPurificacion",
                columns: table => new
                {
                    IdSistema = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreSistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreFabricante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlImagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
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
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nivel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
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
                name: "ComponentesSistema",
                columns: table => new
                {
                    IdComponente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreComponente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    IdProveedor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentesSistema", x => x.IdComponente);
                    table.ForeignKey(
                        name: "FK_ComponentesSistema_Proveedores_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "IdProveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    IdComentario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComentarioTexto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Respuesta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.IdComentario);
                    table.ForeignKey(
                        name: "FK_Comentarios_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioSistemas",
                columns: table => new
                {
                    IdUsuarioSistema = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdSistema = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioSistemas", x => x.IdUsuarioSistema);
                    table.ForeignKey(
                        name: "FK_UsuarioSistemas_SistemasPurificacion_IdSistema",
                        column: x => x.IdSistema,
                        principalTable: "SistemasPurificacion",
                        principalColumn: "IdSistema",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioSistemas_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    IdVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdVendedor = table.Column<int>(type: "int", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventas", x => x.IdVenta);
                    table.ForeignKey(
                        name: "FK_Ventas_Usuarios_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ventas_Usuarios_IdVendedor",
                        column: x => x.IdVendedor,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SistemaRequerimientos",
                columns: table => new
                {
                    IdSistemaComponente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSistema = table.Column<int>(type: "int", nullable: false),
                    IdComponente = table.Column<int>(type: "int", nullable: false),
                    CantidadRequerida = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SistemaRequerimientos", x => x.IdSistemaComponente);
                    table.ForeignKey(
                        name: "FK_SistemaRequerimientos_ComponentesSistema_IdComponente",
                        column: x => x.IdComponente,
                        principalTable: "ComponentesSistema",
                        principalColumn: "IdComponente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SistemaRequerimientos_SistemasPurificacion_IdSistema",
                        column: x => x.IdSistema,
                        principalTable: "SistemasPurificacion",
                        principalColumn: "IdSistema",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mediciones",
                columns: table => new
                {
                    IdMedicion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ph = table.Column<float>(type: "real", nullable: true),
                    NivelTurbidez = table.Column<float>(type: "real", nullable: true),
                    Temperatura = table.Column<float>(type: "real", nullable: true),
                    IdFuente = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioSistema = table.Column<int>(type: "int", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Mediciones_UsuarioSistemas_IdUsuarioSistema",
                        column: x => x.IdUsuarioSistema,
                        principalTable: "UsuarioSistemas",
                        principalColumn: "IdUsuarioSistema",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesVentas",
                columns: table => new
                {
                    IdDetalleVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVenta = table.Column<int>(type: "int", nullable: false),
                    IdSistema = table.Column<int>(type: "int", nullable: true),
                    IdComponente = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Nota = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesVentas", x => x.IdDetalleVenta);
                    table.ForeignKey(
                        name: "FK_DetallesVentas_ComponentesSistema_IdComponente",
                        column: x => x.IdComponente,
                        principalTable: "ComponentesSistema",
                        principalColumn: "IdComponente");
                    table.ForeignKey(
                        name: "FK_DetallesVentas_SistemasPurificacion_IdSistema",
                        column: x => x.IdSistema,
                        principalTable: "SistemasPurificacion",
                        principalColumn: "IdSistema");
                    table.ForeignKey(
                        name: "FK_DetallesVentas_Ventas_IdVenta",
                        column: x => x.IdVenta,
                        principalTable: "Ventas",
                        principalColumn: "IdVenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_IdFuente",
                table: "Alertas",
                column: "IdFuente");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_IdUsuario",
                table: "Comentarios",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentesSistema_IdProveedor",
                table: "ComponentesSistema",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVentas_IdComponente",
                table: "DetallesVentas",
                column: "IdComponente");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVentas_IdSistema",
                table: "DetallesVentas",
                column: "IdSistema");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVentas_IdVenta",
                table: "DetallesVentas",
                column: "IdVenta");

            migrationBuilder.CreateIndex(
                name: "IX_Mediciones_IdFuente",
                table: "Mediciones",
                column: "IdFuente");

            migrationBuilder.CreateIndex(
                name: "IX_Mediciones_IdUsuarioSistema",
                table: "Mediciones",
                column: "IdUsuarioSistema");

            migrationBuilder.CreateIndex(
                name: "IX_SistemaRequerimientos_IdComponente",
                table: "SistemaRequerimientos",
                column: "IdComponente");

            migrationBuilder.CreateIndex(
                name: "IX_SistemaRequerimientos_IdSistema",
                table: "SistemaRequerimientos",
                column: "IdSistema");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioSistemas_IdSistema",
                table: "UsuarioSistemas",
                column: "IdSistema");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioSistemas_IdUsuario",
                table: "UsuarioSistemas",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_IdCliente",
                table: "Ventas",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_IdVendedor",
                table: "Ventas",
                column: "IdVendedor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "Cotizaciones");

            migrationBuilder.DropTable(
                name: "DetallesVentas");

            migrationBuilder.DropTable(
                name: "Mediciones");

            migrationBuilder.DropTable(
                name: "SistemaRequerimientos");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "FuentesAgua");

            migrationBuilder.DropTable(
                name: "UsuarioSistemas");

            migrationBuilder.DropTable(
                name: "ComponentesSistema");

            migrationBuilder.DropTable(
                name: "SistemasPurificacion");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Proveedores");
        }
    }
}
