using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HidroSense.Migrations
{
    public partial class FixComentario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Respuesta",
                table: "Comentarios");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Respuestas",
                table: "Comentarios",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
