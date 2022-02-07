﻿// <auto-generated />
using System;
using LicnostService.Entities.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LicnostService.Migrations
{
    [DbContext(typeof(LicnostContext))]
    [Migration("20220207183603_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KomisijaLicnost", b =>
                {
                    b.Property<Guid>("ClanoviKomisijeLicnostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("KomisijeKomisijaId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ClanoviKomisijeLicnostId", "KomisijeKomisijaId");

                    b.HasIndex("KomisijeKomisijaId");

                    b.ToTable("KomisijaLicnost");

                    b.HasData(
                        new
                        {
                            ClanoviKomisijeLicnostId = new Guid("e3db1e95-c4db-4e11-ac52-9b9e26207e1c"),
                            KomisijeKomisijaId = new Guid("289d3f53-8584-49e1-9511-d994a6dda9a9")
                        },
                        new
                        {
                            ClanoviKomisijeLicnostId = new Guid("95dbbc3b-b961-4364-9bf5-b6f2b4256393"),
                            KomisijeKomisijaId = new Guid("289d3f53-8584-49e1-9511-d994a6dda9a9")
                        },
                        new
                        {
                            ClanoviKomisijeLicnostId = new Guid("e3db1e95-c4db-4e11-ac52-9b9e26207e1c"),
                            KomisijeKomisijaId = new Guid("25410d44-6d96-486e-afd5-1409b906b3de")
                        });
                });

            modelBuilder.Entity("LicnostService.Entities.Komisija", b =>
                {
                    b.Property<Guid>("KomisijaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DokumentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NazivKomisije")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PredsednikKomisijeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("KomisijaId");

                    b.HasIndex("PredsednikKomisijeId");

                    b.ToTable("Komisija");

                    b.HasData(
                        new
                        {
                            KomisijaId = new Guid("25410d44-6d96-486e-afd5-1409b906b3de"),
                            NazivKomisije = "Subotica_Komisija",
                            PredsednikKomisijeId = new Guid("95dbbc3b-b961-4364-9bf5-b6f2b4256393")
                        },
                        new
                        {
                            KomisijaId = new Guid("8b2e2f05-3796-444a-9dc2-2c62372c0fa9"),
                            NazivKomisije = "Subotica_Komisija2",
                            PredsednikKomisijeId = new Guid("4da64d71-ee63-4886-bcd3-fb7ae004a384")
                        },
                        new
                        {
                            KomisijaId = new Guid("289d3f53-8584-49e1-9511-d994a6dda9a9"),
                            NazivKomisije = "Subotica_Komisija",
                            PredsednikKomisijeId = new Guid("4da64d71-ee63-4886-bcd3-fb7ae004a384")
                        });
                });

            modelBuilder.Entity("LicnostService.Entities.Licnost", b =>
                {
                    b.Property<Guid>("LicnostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Funkcija")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prezime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LicnostId");

                    b.ToTable("Licnost");

                    b.HasData(
                        new
                        {
                            LicnostId = new Guid("95dbbc3b-b961-4364-9bf5-b6f2b4256393"),
                            Funkcija = "Sef",
                            Ime = "Stefan",
                            Prezime = "Bulaja"
                        },
                        new
                        {
                            LicnostId = new Guid("4da64d71-ee63-4886-bcd3-fb7ae004a384"),
                            Funkcija = "Licitant",
                            Ime = "Marko",
                            Prezime = "Markovic"
                        },
                        new
                        {
                            LicnostId = new Guid("e3db1e95-c4db-4e11-ac52-9b9e26207e1c"),
                            Funkcija = "Licitant",
                            Ime = "Pera",
                            Prezime = "Peric"
                        });
                });

            modelBuilder.Entity("KomisijaLicnost", b =>
                {
                    b.HasOne("LicnostService.Entities.Licnost", null)
                        .WithMany()
                        .HasForeignKey("ClanoviKomisijeLicnostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LicnostService.Entities.Komisija", null)
                        .WithMany()
                        .HasForeignKey("KomisijeKomisijaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LicnostService.Entities.Komisija", b =>
                {
                    b.HasOne("LicnostService.Entities.Licnost", "PredsednikKomisije")
                        .WithMany()
                        .HasForeignKey("PredsednikKomisijeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PredsednikKomisije");
                });
#pragma warning restore 612, 618
        }
    }
}