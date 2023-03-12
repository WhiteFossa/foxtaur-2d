using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoxtaurServer.Migrations.MainDb
{
    /// <inheritdoc />
    public partial class RemovedExpectedFoxesOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistancesToExpectedFoxesOrderLocations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DistancesToExpectedFoxesOrderLocations",
                columns: table => new
                {
                    AsExpectedFoxOrderLocationInDistancesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpectedFoxesOrderLocationsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistancesToExpectedFoxesOrderLocations", x => new { x.AsExpectedFoxOrderLocationInDistancesId, x.ExpectedFoxesOrderLocationsId });
                    table.ForeignKey(
                        name: "FK_DistancesToExpectedFoxesOrderLocations_Distances_AsExpected~",
                        column: x => x.AsExpectedFoxOrderLocationInDistancesId,
                        principalTable: "Distances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistancesToExpectedFoxesOrderLocations_Locations_ExpectedFo~",
                        column: x => x.ExpectedFoxesOrderLocationsId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistancesToExpectedFoxesOrderLocations_ExpectedFoxesOrderLo~",
                table: "DistancesToExpectedFoxesOrderLocations",
                column: "ExpectedFoxesOrderLocationsId");
        }
    }
}
