﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UplataService.Entities.DataContext;

namespace UplataService.Migrations
{
    [DbContext(typeof(UplataContext))]
    partial class UplataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("UplataService.Entities.Uplata", b =>
                {
                    b.Property<Guid>("UplataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BrojRacuna")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("");

                    b.Property<DateTime>("DatumUplate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Iznos")
                        .HasColumnType("float");

                    b.Property<Guid?>("JavnoNadmetanjeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PozivNaBroj")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasDefaultValue("");

                    b.Property<string>("SvrhaUplate")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasDefaultValue("");

                    b.HasKey("UplataId");

                    b.ToTable("Uplata");

                    b.HasData(
                        new
                        {
                            UplataId = new Guid("8e297ad0-9072-4941-b951-5970eaed18f3"),
                            BrojRacuna = "100-4777487000005-66",
                            DatumUplate = new DateTime(2020, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Iznos = 1999.99,
                            PozivNaBroj = "90-555258-552-559",
                            SvrhaUplate = "Uplata na racun"
                        },
                        new
                        {
                            UplataId = new Guid("19d28646-6779-4896-a9ff-6e7b7b70d87a"),
                            BrojRacuna = "150-2541485965214-99",
                            DatumUplate = new DateTime(2020, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Iznos = 2052.4699999999998,
                            PozivNaBroj = "90-555258-552-559",
                            SvrhaUplate = "Uplata na racun"
                        },
                        new
                        {
                            UplataId = new Guid("633140e2-3fbc-4402-bd4d-ec6e06ec6627"),
                            BrojRacuna = "150-3333385965214-99",
                            DatumUplate = new DateTime(2020, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Iznos = 8482.9899999999998,
                            PozivNaBroj = "90-555258-552-559",
                            SvrhaUplate = "Uplata na racun"
                        });
                });

            modelBuilder.Entity("UplataService.Entities.Uplata", b =>
                {
                    b.OwnsOne("UplataService.Entities.Kurs", "Kurs", b1 =>
                        {
                            b1.Property<Guid>("UplataId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("VrednostKursa")
                                .HasColumnType("float");

                            b1.HasKey("UplataId");

                            b1.ToTable("Uplata");

                            b1.WithOwner()
                                .HasForeignKey("UplataId");
                        });

                    b.Navigation("Kurs");
                });
#pragma warning restore 612, 618
        }
    }
}
