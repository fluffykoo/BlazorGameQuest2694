using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorGame.Api.Migrations
{
    /// <inheritdoc />
    public partial class V4_EstActif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EstActif",
                table: "Joueurs",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstActif",
                table: "Joueurs");
        }
    }
}
