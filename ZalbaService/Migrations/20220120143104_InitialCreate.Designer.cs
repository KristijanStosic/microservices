﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZalbaService.Entities.DataContext;

namespace ZalbaService.Migrations
{
    [DbContext(typeof(ZalbaContext))]
    [Migration("20220120143104_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ZalbaService.Entities.RadnjaZaZalbu", b =>
                {
                    b.Property<Guid>("RadnjaZaZalbuId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NazivRadnjeZaZalbu")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RadnjaZaZalbuId");

                    b.HasIndex("NazivRadnjeZaZalbu")
                        .IsUnique();

                    b.ToTable("RadnjaZaZalbu");

                    b.HasData(
                        new
                        {
                            RadnjaZaZalbuId = new Guid("009aa493-7786-4aad-9d1a-0f90d57ebbb4"),
                            NazivRadnjeZaZalbu = "JN ide u drugi krug sa novim uslovima"
                        },
                        new
                        {
                            RadnjaZaZalbuId = new Guid("4ccb6d66-18b2-4791-8afe-b628a4f7c0af"),
                            NazivRadnjeZaZalbu = "JN ide u drugi krug sa starim uslovima"
                        },
                        new
                        {
                            RadnjaZaZalbuId = new Guid("df645dd7-3e65-41cd-a1f4-81f936a7db49"),
                            NazivRadnjeZaZalbu = "JN ne ide u drugi krug"
                        });
                });

            modelBuilder.Entity("ZalbaService.Entities.StatusZalbe", b =>
                {
                    b.Property<Guid>("StatusZalbeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NazivStatusaZalbe")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("StatusZalbeId");

                    b.HasIndex("NazivStatusaZalbe")
                        .IsUnique();

                    b.ToTable("StatusZalbe");

                    b.HasData(
                        new
                        {
                            StatusZalbeId = new Guid("6e5e8a67-006b-4ac0-89d0-9711006c0d28"),
                            NazivStatusaZalbe = "Usvojena"
                        },
                        new
                        {
                            StatusZalbeId = new Guid("abdb833b-0706-4012-8689-c59bed59c6b4"),
                            NazivStatusaZalbe = "Odbijena"
                        },
                        new
                        {
                            StatusZalbeId = new Guid("2c1051d2-0ddf-41a9-ba08-24070a50f4b3"),
                            NazivStatusaZalbe = "Otvorena"
                        });
                });

            modelBuilder.Entity("ZalbaService.Entities.TipZalbe", b =>
                {
                    b.Property<Guid>("TipZalbeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NazivTipaZalbe")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TipZalbeId");

                    b.HasIndex("NazivTipaZalbe")
                        .IsUnique();

                    b.ToTable("TipZalbe");

                    b.HasData(
                        new
                        {
                            TipZalbeId = new Guid("10ea64f1-07ab-478d-b8f3-073bce4610f8"),
                            NazivTipaZalbe = "Zalba na tok javnog nadmetanja"
                        },
                        new
                        {
                            TipZalbeId = new Guid("b4947a43-42e4-4d20-a10b-169c5089aac6"),
                            NazivTipaZalbe = "Zalba na Odluku o davanju u zakup"
                        },
                        new
                        {
                            TipZalbeId = new Guid("018ac715-4588-4934-bb2d-8bb2f4d1049a"),
                            NazivTipaZalbe = "Zalba na Odluku o davanju na koriscenje"
                        });
                });

            modelBuilder.Entity("ZalbaService.Entities.Zalba", b =>
                {
                    b.Property<Guid>("ZalbaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BrojNadmetanja")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BrojResenja")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DatumPodnosenja")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DatumResenja")
                        .HasColumnType("datetime2");

                    b.Property<string>("Obrazlozenje")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RadnjaZaZalbuId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RazlogZalbe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("StatusZalbeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TipZalbeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ZalbaId");

                    b.HasIndex("RadnjaZaZalbuId");

                    b.HasIndex("StatusZalbeId");

                    b.HasIndex("TipZalbeId");

                    b.ToTable("Zalba");

                    b.HasData(
                        new
                        {
                            ZalbaId = new Guid("26313a1b-0523-4638-957e-c7bc3015cf76"),
                            BrojNadmetanja = "100-NN",
                            BrojResenja = "X9NN41HH",
                            DatumPodnosenja = new DateTime(2022, 1, 20, 15, 31, 4, 201, DateTimeKind.Local).AddTicks(6535),
                            DatumResenja = new DateTime(2022, 3, 11, 15, 31, 4, 204, DateTimeKind.Local).AddTicks(6983),
                            Obrazlozenje = "Nema dovoljno licitanata da se odrzi javno nadmetanje",
                            RadnjaZaZalbuId = new Guid("009aa493-7786-4aad-9d1a-0f90d57ebbb4"),
                            RazlogZalbe = "Nedovoljno licitanata",
                            StatusZalbeId = new Guid("6e5e8a67-006b-4ac0-89d0-9711006c0d28"),
                            TipZalbeId = new Guid("018ac715-4588-4934-bb2d-8bb2f4d1049a")
                        },
                        new
                        {
                            ZalbaId = new Guid("d81e3601-21b1-40cf-a595-1331d0e5212f"),
                            BrojNadmetanja = "200-OO",
                            BrojResenja = "IIKK-55",
                            DatumPodnosenja = new DateTime(2022, 1, 20, 15, 31, 4, 205, DateTimeKind.Local).AddTicks(44),
                            DatumResenja = new DateTime(2022, 3, 11, 15, 31, 4, 205, DateTimeKind.Local).AddTicks(71),
                            Obrazlozenje = "Dokumentacija nije potpuna kako bi se odrzalo javno nadmetanje",
                            RadnjaZaZalbuId = new Guid("009aa493-7786-4aad-9d1a-0f90d57ebbb4"),
                            RazlogZalbe = "Nepotpuna dokumentacija",
                            StatusZalbeId = new Guid("6e5e8a67-006b-4ac0-89d0-9711006c0d28"),
                            TipZalbeId = new Guid("b4947a43-42e4-4d20-a10b-169c5089aac6")
                        },
                        new
                        {
                            ZalbaId = new Guid("7951acaa-ae36-410d-916c-df398713811f"),
                            BrojNadmetanja = "999-AA",
                            BrojResenja = "QWOP44-MM",
                            DatumPodnosenja = new DateTime(2022, 1, 20, 15, 31, 4, 205, DateTimeKind.Local).AddTicks(114),
                            DatumResenja = new DateTime(2022, 3, 11, 15, 31, 4, 205, DateTimeKind.Local).AddTicks(119),
                            Obrazlozenje = "Nema dovoljno novcanih sredstava za javno nadmetanje",
                            RadnjaZaZalbuId = new Guid("4ccb6d66-18b2-4791-8afe-b628a4f7c0af"),
                            RazlogZalbe = "Nedovoljno uplacenih novcanih sredstava",
                            StatusZalbeId = new Guid("2c1051d2-0ddf-41a9-ba08-24070a50f4b3"),
                            TipZalbeId = new Guid("b4947a43-42e4-4d20-a10b-169c5089aac6")
                        });
                });

            modelBuilder.Entity("ZalbaService.Entities.Zalba", b =>
                {
                    b.HasOne("ZalbaService.Entities.RadnjaZaZalbu", "RadnjaZaZalbu")
                        .WithMany()
                        .HasForeignKey("RadnjaZaZalbuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ZalbaService.Entities.StatusZalbe", "StatusZalbe")
                        .WithMany()
                        .HasForeignKey("StatusZalbeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ZalbaService.Entities.TipZalbe", "TipZalbe")
                        .WithMany()
                        .HasForeignKey("TipZalbeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RadnjaZaZalbu");

                    b.Navigation("StatusZalbe");

                    b.Navigation("TipZalbe");
                });
#pragma warning restore 612, 618
        }
    }
}
