using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoxtaurServer.Migrations.MainDb
{
    /// <inheritdoc />
    public partial class AddedHunterProfileAndTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ColorA = table.Column<byte>(type: "smallint", nullable: false),
                    ColorR = table.Column<byte>(type: "smallint", nullable: false),
                    ColorG = table.Column<byte>(type: "smallint", nullable: false),
                    ColorB = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Sex = table.Column<int>(type: "integer", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: true),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    ColorA = table.Column<byte>(type: "smallint", nullable: false),
                    ColorR = table.Column<byte>(type: "smallint", nullable: false),
                    ColorG = table.Column<byte>(type: "smallint", nullable: false),
                    ColorB = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_TeamId",
                table: "Profiles",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
