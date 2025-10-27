using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationServices.Migrations
{
    /// <inheritdoc />
    public partial class AddAdministrateurAndDonjon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DonjonId",
                table: "Salles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DonjonId",
                table: "Parties",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AdministrateurId",
                table: "Joueurs",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Administrateurs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NomUtilisateur = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    MotDePasse = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrateurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donjons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    NombreDeSalles = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donjons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Salles_DonjonId",
                table: "Salles",
                column: "DonjonId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_DonjonId",
                table: "Parties",
                column: "DonjonId");

            migrationBuilder.CreateIndex(
                name: "IX_Joueurs_AdministrateurId",
                table: "Joueurs",
                column: "AdministrateurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Joueurs_Administrateurs_AdministrateurId",
                table: "Joueurs",
                column: "AdministrateurId",
                principalTable: "Administrateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_Donjons_DonjonId",
                table: "Parties",
                column: "DonjonId",
                principalTable: "Donjons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Salles_Donjons_DonjonId",
                table: "Salles",
                column: "DonjonId",
                principalTable: "Donjons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Joueurs_Administrateurs_AdministrateurId",
                table: "Joueurs");

            migrationBuilder.DropForeignKey(
                name: "FK_Parties_Donjons_DonjonId",
                table: "Parties");

            migrationBuilder.DropForeignKey(
                name: "FK_Salles_Donjons_DonjonId",
                table: "Salles");

            migrationBuilder.DropTable(
                name: "Administrateurs");

            migrationBuilder.DropTable(
                name: "Donjons");

            migrationBuilder.DropIndex(
                name: "IX_Salles_DonjonId",
                table: "Salles");

            migrationBuilder.DropIndex(
                name: "IX_Parties_DonjonId",
                table: "Parties");

            migrationBuilder.DropIndex(
                name: "IX_Joueurs_AdministrateurId",
                table: "Joueurs");

            migrationBuilder.DropColumn(
                name: "DonjonId",
                table: "Salles");

            migrationBuilder.DropColumn(
                name: "DonjonId",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "AdministrateurId",
                table: "Joueurs");
        }
    }
}
