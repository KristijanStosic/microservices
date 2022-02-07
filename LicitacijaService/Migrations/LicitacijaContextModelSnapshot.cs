﻿// <auto-generated />
using System;
using LicitacijaService.Entities.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LicitacijaService.Migrations
{
    [DbContext(typeof(LicitacijaContext))]
    partial class LicitacijaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LicitacijaService.Entities.Licitacija", b =>
                {
                    b.Property<Guid>("LicitacijaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BrojLicitacije")
                        .HasColumnType("int");

                    b.Property<int>("GodinaLicitacije")
                        .HasColumnType("int");

                    b.Property<int>("KorakCeneLicitacije")
                        .HasColumnType("int");

                    b.Property<int>("OgranicenjeLicitacije")
                        .HasColumnType("int");

                    b.Property<DateTime>("RokLicitacije")
                        .HasColumnType("datetime2");

                    b.HasKey("LicitacijaId");

                    b.ToTable("Licitacija");

                    b.HasData(
                        new
                        {
                            LicitacijaId = new Guid("cac0e0a2-852a-4265-ac71-49c25fb5559b"),
                            BrojLicitacije = 42,
                            GodinaLicitacije = 2012,
                            KorakCeneLicitacije = 3,
                            OgranicenjeLicitacije = 12,
                            RokLicitacije = new DateTime(2020, 9, 25, 19, 33, 34, 894, DateTimeKind.Local).AddTicks(8874)
                        },
                        new
                        {
                            LicitacijaId = new Guid("2ff32eb3-a7a1-4e8b-a9e1-8ec51f3eca4c"),
                            BrojLicitacije = 43,
                            GodinaLicitacije = 2012,
                            KorakCeneLicitacije = 2,
                            OgranicenjeLicitacije = 13,
                            RokLicitacije = new DateTime(2020, 10, 6, 19, 33, 34, 899, DateTimeKind.Local).AddTicks(5896)
                        },
                        new
                        {
                            LicitacijaId = new Guid("fb96a27d-f87f-49b5-98f3-ef6b57e84c3c"),
                            BrojLicitacije = 44,
                            GodinaLicitacije = 2012,
                            KorakCeneLicitacije = 4,
                            OgranicenjeLicitacije = 14,
                            RokLicitacije = new DateTime(2020, 10, 15, 19, 33, 34, 899, DateTimeKind.Local).AddTicks(5964)
                        });
                });

            modelBuilder.Entity("LicitacijaService.Entities.ManyToMany.LicitacijaJavnoNadmetanje", b =>
                {
                    b.Property<Guid>("LicitacijaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("JavnoNadmetanjeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LicitacijaId", "JavnoNadmetanjeId");

                    b.ToTable("LicitacijaJavnoNadmetanje");

                    b.HasData(
                        new
                        {
                            LicitacijaId = new Guid("cac0e0a2-852a-4265-ac71-49c25fb5559b"),
                            JavnoNadmetanjeId = new Guid("56a7cff5-cb0a-46b8-bfc1-93db415feeb4")
                        },
                        new
                        {
                            LicitacijaId = new Guid("cac0e0a2-852a-4265-ac71-49c25fb5559b"),
                            JavnoNadmetanjeId = new Guid("6849bbbe-5798-4c04-aa20-58de420aa578")
                        },
                        new
                        {
                            LicitacijaId = new Guid("2ff32eb3-a7a1-4e8b-a9e1-8ec51f3eca4c"),
                            JavnoNadmetanjeId = new Guid("b195c4c2-2b26-40ad-8ff6-c212474acc83")
                        });
                });

            modelBuilder.Entity("LicitacijaService.Entities.ManyToMany.LicitacijaJavnoNadmetanje", b =>
                {
                    b.HasOne("LicitacijaService.Entities.Licitacija", "Licitacija")
                        .WithMany()
                        .HasForeignKey("LicitacijaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Licitacija");
                });
#pragma warning restore 612, 618
        }
    }
}