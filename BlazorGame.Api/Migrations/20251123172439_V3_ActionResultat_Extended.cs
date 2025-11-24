using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorGame.Api.Migrations
{
    /// <inheritdoc />
    public partial class V3_ActionResultat_Extended : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Resultat_Risque",
                table: "Salles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Resultat_ScoreTotal",
                table: "Salles",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resultat_Risque",
                table: "Salles");

            migrationBuilder.DropColumn(
                name: "Resultat_ScoreTotal",
                table: "Salles");
        }
    }
}
