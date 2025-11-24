using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorGame.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddEstVisiteeToSalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salles_Donjons_DonjonId",
                table: "Salles");

            migrationBuilder.AlterColumn<Guid>(
                name: "DonjonId",
                table: "Salles",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<bool>(
                name: "EstVisitee",
                table: "Salles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ForceMonstre",
                table: "Salles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageMonstre",
                table: "Salles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomMonstre",
                table: "Salles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PvMonstre",
                table: "Salles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Salles_Donjons_DonjonId",
                table: "Salles",
                column: "DonjonId",
                principalTable: "Donjons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salles_Donjons_DonjonId",
                table: "Salles");

            migrationBuilder.DropColumn(
                name: "EstVisitee",
                table: "Salles");

            migrationBuilder.DropColumn(
                name: "ForceMonstre",
                table: "Salles");

            migrationBuilder.DropColumn(
                name: "ImageMonstre",
                table: "Salles");

            migrationBuilder.DropColumn(
                name: "NomMonstre",
                table: "Salles");

            migrationBuilder.DropColumn(
                name: "PvMonstre",
                table: "Salles");

            migrationBuilder.AlterColumn<Guid>(
                name: "DonjonId",
                table: "Salles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Salles_Donjons_DonjonId",
                table: "Salles",
                column: "DonjonId",
                principalTable: "Donjons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
