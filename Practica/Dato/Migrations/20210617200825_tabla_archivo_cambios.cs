using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class tabla_archivo_cambios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Archivo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Archivo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Archivo_UsuarioId",
                table: "Archivo",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Archivo_User_UsuarioId",
                table: "Archivo",
                column: "UsuarioId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Archivo_User_UsuarioId",
                table: "Archivo");

            migrationBuilder.DropIndex(
                name: "IX_Archivo_UsuarioId",
                table: "Archivo");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Archivo");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Archivo");
        }
    }
}
