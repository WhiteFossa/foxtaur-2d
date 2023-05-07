using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoxtaurServer.Migrations.MainDb
{
    /// <inheritdoc />
    public partial class AddMapFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "Maps",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MapFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    LocalPath = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Maps_FileId",
                table: "Maps",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_MapFiles_FileId",
                table: "Maps",
                column: "FileId",
                principalTable: "MapFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
