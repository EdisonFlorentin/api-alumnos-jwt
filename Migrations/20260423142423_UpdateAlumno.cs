using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiAlumnos.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAlumno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Curso",
                table: "Alumnos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Curso",
                table: "Alumnos");
        }
    }
}
