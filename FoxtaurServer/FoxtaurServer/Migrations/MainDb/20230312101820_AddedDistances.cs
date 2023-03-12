using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoxtaurServer.Migrations.MainDb
{
    /// <inheritdoc />
    public partial class AddedDistances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Distances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    MapId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    StartLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    FinishCorridorEntranceLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    FinishLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    FirstHunterStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Distances_Locations_FinishCorridorEntranceLocationId",
                        column: x => x.FinishCorridorEntranceLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Distances_Locations_FinishLocationId",
                        column: x => x.FinishLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Distances_Locations_StartLocationId",
                        column: x => x.StartLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Distances_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DistanceProfile",
                columns: table => new
                {
                    HuntersId = table.Column<string>(type: "text", nullable: false),
                    ParticipatedInDistancesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistanceProfile", x => new { x.HuntersId, x.ParticipatedInDistancesId });
                    table.ForeignKey(
                        name: "FK_DistanceProfile_Distances_ParticipatedInDistancesId",
                        column: x => x.ParticipatedInDistancesId,
                        principalTable: "Distances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistanceProfile_Profiles_HuntersId",
                        column: x => x.HuntersId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "DistancesToFoxesLocations",
                columns: table => new
                {
                    AsFoxLocationInDistancesId = table.Column<Guid>(type: "uuid", nullable: false),
                    FoxesLocationsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistancesToFoxesLocations", x => new { x.AsFoxLocationInDistancesId, x.FoxesLocationsId });
                    table.ForeignKey(
                        name: "FK_DistancesToFoxesLocations_Distances_AsFoxLocationInDistance~",
                        column: x => x.AsFoxLocationInDistancesId,
                        principalTable: "Distances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistancesToFoxesLocations_Locations_FoxesLocationsId",
                        column: x => x.FoxesLocationsId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistanceProfile_ParticipatedInDistancesId",
                table: "DistanceProfile",
                column: "ParticipatedInDistancesId");

            migrationBuilder.CreateIndex(
                name: "IX_Distances_FinishCorridorEntranceLocationId",
                table: "Distances",
                column: "FinishCorridorEntranceLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Distances_FinishLocationId",
                table: "Distances",
                column: "FinishLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Distances_MapId",
                table: "Distances",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Distances_StartLocationId",
                table: "Distances",
                column: "StartLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DistancesToExpectedFoxesOrderLocations_ExpectedFoxesOrderLo~",
                table: "DistancesToExpectedFoxesOrderLocations",
                column: "ExpectedFoxesOrderLocationsId");

            migrationBuilder.CreateIndex(
                name: "IX_DistancesToFoxesLocations_FoxesLocationsId",
                table: "DistancesToFoxesLocations",
                column: "FoxesLocationsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
