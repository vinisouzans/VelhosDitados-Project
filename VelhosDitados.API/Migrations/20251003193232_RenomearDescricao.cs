using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VelhosDitados.API.Migrations
{
    /// <inheritdoc />
    public partial class RenomearDescricao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descrição",
                table: "Ditados",
                newName: "Descricao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "Ditados",
                newName: "Descrição");
        }
    }
}
