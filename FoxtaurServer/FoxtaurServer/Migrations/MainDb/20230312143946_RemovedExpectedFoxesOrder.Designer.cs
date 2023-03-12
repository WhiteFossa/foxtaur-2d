﻿// <auto-generated />
using System;
using FoxtaurServer.Dao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FoxtaurServer.Migrations.MainDb
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20230312143946_RemovedExpectedFoxesOrder")]
    partial class RemovedExpectedFoxesOrder
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DistanceLocation", b =>
                {
                    b.Property<Guid>("AsFoxLocationInDistancesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FoxesLocationsId")
                        .HasColumnType("uuid");

                    b.HasKey("AsFoxLocationInDistancesId", "FoxesLocationsId");

                    b.HasIndex("FoxesLocationsId");

                    b.ToTable("DistancesToFoxesLocations", (string)null);
                });

            modelBuilder.Entity("DistanceProfile", b =>
                {
                    b.Property<string>("HuntersId")
                        .HasColumnType("text");

                    b.Property<Guid>("ParticipatedInDistancesId")
                        .HasColumnType("uuid");

                    b.HasKey("HuntersId", "ParticipatedInDistancesId");

                    b.HasIndex("ParticipatedInDistancesId");

                    b.ToTable("DistanceProfile");
                });

            modelBuilder.Entity("FoxtaurServer.Dao.Models.Distance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("FinishCorridorEntranceLocationId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("FinishLocationId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("FirstHunterStartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("MapId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("StartLocationId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FinishCorridorEntranceLocationId");

                    b.HasIndex("FinishLocationId");

                    b.HasIndex("MapId");

                    b.HasIndex("StartLocationId");

                    b.ToTable("Distances");
                });

            modelBuilder.Entity("FoxtaurServer.Dao.Models.Fox", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<double>("Frequency")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Foxes");
                });

            modelBuilder.Entity("FoxtaurServer.Dao.Models.HunterLocation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Alt")
                        .HasColumnType("double precision");

                    b.Property<string>("HunterId")
                        .HasColumnType("text");

                    b.Property<double>("Lat")
                        .HasColumnType("double precision");

                    b.Property<double>("Lon")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("HunterId");

                    b.ToTable("HuntersLocations");
                });

            modelBuilder.Entity("FoxtaurServer.Dao.Models.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("FoxId")
                        .HasColumnType("uuid");

                    b.Property<double>("Lat")
                        .HasColumnType("double precision");

                    b.Property<double>("Lon")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FoxId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("FoxtaurServer.Dao.Models.Map", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("EastLon")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<double>("NorthLat")
                        .HasColumnType("double precision");

                    b.Property<double>("SouthLat")
                        .HasColumnType("double precision");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.Property<double>("WestLon")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("FoxtaurServer.Dao.Models.Profile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<byte>("ColorA")
                        .HasColumnType("smallint");

                    b.Property<byte>("ColorB")
                        .HasColumnType("smallint");

                    b.Property<byte>("ColorG")
                        .HasColumnType("smallint");

                    b.Property<byte>("ColorR")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<int>("Sex")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("FoxtaurServer.Dao.Models.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte>("ColorA")
                        .HasColumnType("smallint");

                    b.Property<byte>("ColorB")
                        .HasColumnType("smallint");

                    b.Property<byte>("ColorG")
                        .HasColumnType("smallint");

                    b.Property<byte>("ColorR")
                        .HasColumnType("smallint");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("DistanceLocation", b =>
                {
                    b.HasOne("FoxtaurServer.Dao.Models.Distance", null)
                        .WithMany()
                        .HasForeignKey("AsFoxLocationInDistancesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoxtaurServer.Dao.Models.Location", null)
                        .WithMany()
                        .HasForeignKey("FoxesLocationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DistanceProfile", b =>
                {
                    b.HasOne("FoxtaurServer.Dao.Models.Profile", null)
                        .WithMany()
                        .HasForeignKey("HuntersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoxtaurServer.Dao.Models.Distance", null)
                        .WithMany()
                        .HasForeignKey("ParticipatedInDistancesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FoxtaurServer.Dao.Models.Distance", b =>
                {
                    b.HasOne("FoxtaurServer.Dao.Models.Location", "FinishCorridorEntranceLocation")
                        .WithMany("AsFinishCorridorEntranceInDistances")
                        .HasForeignKey("FinishCorridorEntranceLocationId");

                    b.HasOne("FoxtaurServer.Dao.Models.Location", "FinishLocation")
                        .WithMany("AsFinishLocationInDistances")
                        .HasForeignKey("FinishLocationId");

                    b.HasOne("FoxtaurServer.Dao.Models.Map", "Map")
                        .WithMany()
                        .HasForeignKey("MapId");

                    b.HasOne("FoxtaurServer.Dao.Models.Location", "StartLocation")
                        .WithMany("AsStartInDistances")
                        .HasForeignKey("StartLocationId");

                    b.Navigation("FinishCorridorEntranceLocation");

                    b.Navigation("FinishLocation");

                    b.Navigation("Map");

                    b.Navigation("StartLocation");
                });

            modelBuilder.Entity("FoxtaurServer.Dao.Models.HunterLocation", b =>
                {
                    b.HasOne("FoxtaurServer.Dao.Models.Profile", "Hunter")
                        .WithMany()
                        .HasForeignKey("HunterId");

                    b.Navigation("Hunter");
                });

            modelBuilder.Entity("FoxtaurServer.Dao.Models.Location", b =>
                {
                    b.HasOne("FoxtaurServer.Dao.Models.Fox", "Fox")
                        .WithMany()
                        .HasForeignKey("FoxId");

                    b.Navigation("Fox");
                });

            modelBuilder.Entity("FoxtaurServer.Dao.Models.Profile", b =>
                {
                    b.HasOne("FoxtaurServer.Dao.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("FoxtaurServer.Dao.Models.Location", b =>
                {
                    b.Navigation("AsFinishCorridorEntranceInDistances");

                    b.Navigation("AsFinishLocationInDistances");

                    b.Navigation("AsStartInDistances");
                });
#pragma warning restore 612, 618
        }
    }
}
