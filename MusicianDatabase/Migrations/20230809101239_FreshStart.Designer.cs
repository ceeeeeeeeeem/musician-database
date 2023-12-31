﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicianDatabase.Data;

#nullable disable

namespace MusicianDatabase.Migrations
{
    [DbContext(typeof(MusicianDbContext))]
    [Migration("20230809101239_FreshStart")]
    partial class FreshStart
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MusicianDatabase.Artist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("MusicianDatabase.Band", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Bands");
                });

            modelBuilder.Entity("MusicianDatabase.Concert", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VenueId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VenueId");

                    b.ToTable("Concert");
                });

            modelBuilder.Entity("MusicianDatabase.ConcertBand", b =>
                {
                    b.Property<int>("ConcertId")
                        .HasColumnType("int");

                    b.Property<int>("BandId")
                        .HasColumnType("int");

                    b.HasKey("ConcertId", "BandId");

                    b.HasIndex("BandId");

                    b.ToTable("ConcertBand");
                });

            modelBuilder.Entity("MusicianDatabase.Instrument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Instruments");
                });

            modelBuilder.Entity("MusicianDatabase.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ArtistId")
                        .HasColumnType("int");

                    b.Property<int>("BandId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("BandId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("MusicianDatabase.RoleInstruments", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("InstrumentId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "InstrumentId");

                    b.HasIndex("InstrumentId");

                    b.ToTable("RoleInstruments");
                });

            modelBuilder.Entity("MusicianDatabase.Venue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Venue");
                });

            modelBuilder.Entity("MusicianDatabase.Concert", b =>
                {
                    b.HasOne("MusicianDatabase.Venue", "Venue")
                        .WithMany("Concerts")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("MusicianDatabase.ConcertBand", b =>
                {
                    b.HasOne("MusicianDatabase.Band", "Band")
                        .WithMany("ConcertBands")
                        .HasForeignKey("BandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicianDatabase.Concert", "Concert")
                        .WithMany("ConcertBands")
                        .HasForeignKey("ConcertId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Band");

                    b.Navigation("Concert");
                });

            modelBuilder.Entity("MusicianDatabase.Role", b =>
                {
                    b.HasOne("MusicianDatabase.Artist", "Artist")
                        .WithMany("Roles")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicianDatabase.Band", "Band")
                        .WithMany("Roles")
                        .HasForeignKey("BandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Band");
                });

            modelBuilder.Entity("MusicianDatabase.RoleInstruments", b =>
                {
                    b.HasOne("MusicianDatabase.Instrument", "Instrument")
                        .WithMany("RoleInstruments")
                        .HasForeignKey("InstrumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicianDatabase.Role", "Role")
                        .WithMany("RoleInstruments")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instrument");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("MusicianDatabase.Artist", b =>
                {
                    b.Navigation("Roles");
                });

            modelBuilder.Entity("MusicianDatabase.Band", b =>
                {
                    b.Navigation("ConcertBands");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("MusicianDatabase.Concert", b =>
                {
                    b.Navigation("ConcertBands");
                });

            modelBuilder.Entity("MusicianDatabase.Instrument", b =>
                {
                    b.Navigation("RoleInstruments");
                });

            modelBuilder.Entity("MusicianDatabase.Role", b =>
                {
                    b.Navigation("RoleInstruments");
                });

            modelBuilder.Entity("MusicianDatabase.Venue", b =>
                {
                    b.Navigation("Concerts");
                });
#pragma warning restore 612, 618
        }
    }
}
