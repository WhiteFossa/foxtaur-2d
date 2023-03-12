using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoxtaurServer.Migrations.MainDb
{
    /// <inheritdoc />
    public partial class AddedDistancesToLocationsLinker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistancesToFoxesLocations");

            migrationBuilder.CreateTable(
                name: "DistanceToFoxLocationLinker",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistanceId = table.Column<Guid>(type: "uuid", nullable: true),
                    FoxLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistanceToFoxLocationLinker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistanceToFoxLocationLinker_Distances_DistanceId",
                        column: x => x.DistanceId,
                        principalTable: "Distances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DistanceToFoxLocationLinker_Locations_FoxLocationId",
                        column: x => x.FoxLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistanceToFoxLocationLinker_DistanceId",
                table: "DistanceToFoxLocationLinker",
                column: "DistanceId");

            migrationBuilder.CreateIndex(
                name: "IX_DistanceToFoxLocationLinker_FoxLocationId",
                table: "DistanceToFoxLocationLinker",
                column: "FoxLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
