using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HidroSense.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNivelYToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nivel",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Usuarios");
        }
    }
}
