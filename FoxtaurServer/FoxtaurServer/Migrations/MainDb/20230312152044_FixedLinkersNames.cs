using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoxtaurServer.Migrations.MainDb
{
    /// <inheritdoc />
    public partial class FixedLinkersNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistanceToFoxLocationLinker_Distances_DistanceId",
                table: "DistanceToFoxLocationLinker");

            migrationBuilder.DropForeignKey(
                name: "FK_DistanceToFoxLocationLinker_Locations_FoxLocationId",
                table: "DistanceToFoxLocationLinker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DistanceToFoxLocationLinker",
                table: "DistanceToFoxLocationLinker");

            migrationBuilder.RenameTable(
                name: "DistanceToFoxLocationLinker",
                newName: "DistanceToFoxLocationLinkers");

            migrationBuilder.RenameIndex(
                name: "IX_DistanceToFoxLocationLinker_FoxLocationId",
                table: "DistanceToFoxLocationLinkers",
                newName: "IX_DistanceToFoxLocationLinkers_FoxLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_DistanceToFoxLocationLinker_DistanceId",
                table: "DistanceToFoxLocationLinkers",
                newName: "IX_DistanceToFoxLocationLinkers_DistanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DistanceToFoxLocationLinkers",
                table: "DistanceToFoxLocationLinkers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DistanceToFoxLocationLinkers_Distances_DistanceId",
                table: "DistanceToFoxLocationLinkers",
                column: "DistanceId",
                principalTable: "Distances",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DistanceToFoxLocationLinkers_Locations_FoxLocationId",
                table: "DistanceToFoxLocationLinkers",
                column: "FoxLocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
