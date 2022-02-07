﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ParcelaService.Entities.DataContext;

namespace ParcelaService.Migrations
{
    [DbContext(typeof(ParcelaContext))]
    partial class ParcelaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ParcelaService.Entities.DeoParcele", b =>
                {
                    b.Property<Guid>("DeoParceleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("KlasaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("KulturaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("KupacId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ObradivostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ParcelaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PovrsinaDela")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RedniBrojDela")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DeoParceleId");

                    b.HasIndex("KlasaId");

                    b.HasIndex("KulturaId");

                    b.HasIndex("ObradivostId");

                    b.HasIndex("ParcelaId");

                    b.ToTable("DeoParcele");

                    b.HasData(
                        new
                        {
                            DeoParceleId = new Guid("17894615-ca22-4943-87c8-16c246a35879"),
                            KlasaId = new Guid("f39c9623-c6b6-48cb-b4dd-0340c7431870"),
                            KulturaId = new Guid("f32bf0e6-cb02-49b0-a035-79e350255742"),
                            KupacId = new Guid("febd1c29-90e7-40c2-97f3-1e88495fe98d"),
                            ObradivostId = new Guid("1c48c1d4-122b-4bd2-a8fe-188e54c5a88a"),
                            ParcelaId = new Guid("73e47b70-c8fb-43e3-beb9-5f1b627a59bf"),
                            PovrsinaDela = "40",
                            RedniBrojDela = "12"
                        },
                        new
                        {
                            DeoParceleId = new Guid("44302280-3611-4667-bcfc-08b4e272bb28"),
                            KlasaId = new Guid("0ec59e12-b271-471f-9a13-5c9c8ed0eda7"),
                            KulturaId = new Guid("dcfc0f60-1683-4f16-919f-b6fbdf361fac"),
                            KupacId = new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"),
                            ObradivostId = new Guid("ec9e3d3e-193d-4de8-bdc4-acc3e4bf834d"),
                            ParcelaId = new Guid("5d99fd91-fcf4-4975-891b-a7ae3053ff52"),
                            PovrsinaDela = "15",
                            RedniBrojDela = "14"
                        },
                        new
                        {
                            DeoParceleId = new Guid("c5bfe49c-518e-4216-a379-a5ec94b9efe2"),
                            KlasaId = new Guid("e496b563-abb9-48a9-8972-800f41a4a3a1"),
                            KulturaId = new Guid("d89d9175-bdf0-4066-850a-4232318f80bb"),
                            KupacId = new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"),
                            ObradivostId = new Guid("0d62386e-e188-49a9-a8e6-492fa14baeb4"),
                            ParcelaId = new Guid("5d99fd91-fcf4-4975-891b-a7ae3053ff52"),
                            PovrsinaDela = "25",
                            RedniBrojDela = "20"
                        });
                });

            modelBuilder.Entity("ParcelaService.Entities.KatastarskaOpstina", b =>
                {
                    b.Property<Guid>("KatastarskaOpstinaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NazivKatastarskeOpstine")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KatastarskaOpstinaId");

                    b.ToTable("KatastarskaOpstina");

                    b.HasData(
                        new
                        {
                            KatastarskaOpstinaId = new Guid("9ef61f9a-a078-423c-96f3-c83bd2d1806f"),
                            NazivKatastarskeOpstine = "Čantavir"
                        },
                        new
                        {
                            KatastarskaOpstinaId = new Guid("2e101ea1-f68d-411c-934d-07970563474d"),
                            NazivKatastarskeOpstine = "Bački Vinogradi"
                        },
                        new
                        {
                            KatastarskaOpstinaId = new Guid("8772f89a-65ed-4240-95a8-cfd06775283b"),
                            NazivKatastarskeOpstine = "Bikovo"
                        },
                        new
                        {
                            KatastarskaOpstinaId = new Guid("ce8db946-e6a1-42e3-96b6-3ca9387a64c4"),
                            NazivKatastarskeOpstine = "Đuđin"
                        },
                        new
                        {
                            KatastarskaOpstinaId = new Guid("2fd6ea35-4bec-41a6-b705-87cb1fc4ff20"),
                            NazivKatastarskeOpstine = "Žednik"
                        },
                        new
                        {
                            KatastarskaOpstinaId = new Guid("e5ff87f4-6a6f-48ac-b3e4-2f324d2eb3cb"),
                            NazivKatastarskeOpstine = "Tavankut"
                        },
                        new
                        {
                            KatastarskaOpstinaId = new Guid("6905b37f-4449-41f4-8ac5-9e416dd17ff0"),
                            NazivKatastarskeOpstine = "Bajmok"
                        },
                        new
                        {
                            KatastarskaOpstinaId = new Guid("60e61296-b612-4950-b2d9-8766b82ebdb5"),
                            NazivKatastarskeOpstine = "Donji Grad"
                        },
                        new
                        {
                            KatastarskaOpstinaId = new Guid("571591d6-9d2c-4ca3-99f6-4f3d7df6863b"),
                            NazivKatastarskeOpstine = "Stari Grad"
                        },
                        new
                        {
                            KatastarskaOpstinaId = new Guid("78a28389-ef8f-4713-9640-ce047d99470e"),
                            NazivKatastarskeOpstine = "Novi Grad"
                        },
                        new
                        {
                            KatastarskaOpstinaId = new Guid("a8670b5a-41bc-4f90-9b37-6b78c06d2fd3"),
                            NazivKatastarskeOpstine = "Palić"
                        });
                });

            modelBuilder.Entity("ParcelaService.Entities.Klasa", b =>
                {
                    b.Property<Guid>("KlasaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("KlasaNaziv")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KlasaId");

                    b.ToTable("Klasa");

                    b.HasData(
                        new
                        {
                            KlasaId = new Guid("f39c9623-c6b6-48cb-b4dd-0340c7431870"),
                            KlasaNaziv = "Prva"
                        },
                        new
                        {
                            KlasaId = new Guid("0ec59e12-b271-471f-9a13-5c9c8ed0eda7"),
                            KlasaNaziv = "Druga"
                        },
                        new
                        {
                            KlasaId = new Guid("e496b563-abb9-48a9-8972-800f41a4a3a1"),
                            KlasaNaziv = "Treca"
                        });
                });

            modelBuilder.Entity("ParcelaService.Entities.Kultura", b =>
                {
                    b.Property<Guid>("KulturaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NazivKulture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KulturaId");

                    b.ToTable("Kultura");

                    b.HasData(
                        new
                        {
                            KulturaId = new Guid("f32bf0e6-cb02-49b0-a035-79e350255742"),
                            NazivKulture = "Njiva"
                        },
                        new
                        {
                            KulturaId = new Guid("dcfc0f60-1683-4f16-919f-b6fbdf361fac"),
                            NazivKulture = "Vinograd"
                        },
                        new
                        {
                            KulturaId = new Guid("d89d9175-bdf0-4066-850a-4232318f80bb"),
                            NazivKulture = "Livada"
                        });
                });

            modelBuilder.Entity("ParcelaService.Entities.OblikSvojine", b =>
                {
                    b.Property<Guid>("OblikSvojineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OpisOblikaSvojine")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OblikSvojineId");

                    b.ToTable("OblikSvojine");

                    b.HasData(
                        new
                        {
                            OblikSvojineId = new Guid("f5133187-104c-4849-9d53-995e17e51094"),
                            OpisOblikaSvojine = "Privatna svojina"
                        },
                        new
                        {
                            OblikSvojineId = new Guid("03e3208e-eb61-40f1-b9d4-36fb3f63e4c6"),
                            OpisOblikaSvojine = "Zajednicka svojina"
                        });
                });

            modelBuilder.Entity("ParcelaService.Entities.Obradivost", b =>
                {
                    b.Property<Guid>("ObradivostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OpisObradivosti")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ObradivostId");

                    b.ToTable("Obradivost");

                    b.HasData(
                        new
                        {
                            ObradivostId = new Guid("1c48c1d4-122b-4bd2-a8fe-188e54c5a88a"),
                            OpisObradivosti = "Moze se raditi redukovana obrada u odredjenoj meri"
                        },
                        new
                        {
                            ObradivostId = new Guid("ec9e3d3e-193d-4de8-bdc4-acc3e4bf834d"),
                            OpisObradivosti = "Ne moze se raditi direktna setva"
                        },
                        new
                        {
                            ObradivostId = new Guid("0d62386e-e188-49a9-a8e6-492fa14baeb4"),
                            OpisObradivosti = "Pogodno gajenje vecine kultura"
                        });
                });

            modelBuilder.Entity("ParcelaService.Entities.Odvodnjavanje", b =>
                {
                    b.Property<Guid>("OdvodnjavanjeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OpisOdvodnjavanja")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OdvodnjavanjeId");

                    b.ToTable("Odvodnjavanje");

                    b.HasData(
                        new
                        {
                            OdvodnjavanjeId = new Guid("b2f93ba3-ec00-41af-824e-fcd1f0c60c5c"),
                            OpisOdvodnjavanja = "Odvodnjavanje na parceli nije potrebno."
                        },
                        new
                        {
                            OdvodnjavanjeId = new Guid("d3c4ebaa-178c-4d62-afbc-05819e041021"),
                            OpisOdvodnjavanja = "Odvodnjavanje na parceli nije potrebno."
                        },
                        new
                        {
                            OdvodnjavanjeId = new Guid("6601b7a7-d1be-4216-9844-8d68d680847e"),
                            OpisOdvodnjavanja = "Odvodnjavanje na parceli nije potrebno."
                        });
                });

            modelBuilder.Entity("ParcelaService.Entities.Parcela", b =>
                {
                    b.Property<Guid>("ParcelaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BrojListeNepokretnosti")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BrojParcele")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("KatastarskaOpstinaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OblikSvojineId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OdvodnjavanjeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PovrsinaParcele")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ZasticenaZonaId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ParcelaId");

                    b.HasIndex("KatastarskaOpstinaId");

                    b.HasIndex("OblikSvojineId");

                    b.HasIndex("OdvodnjavanjeId");

                    b.HasIndex("ZasticenaZonaId");

                    b.ToTable("Parcela");

                    b.HasData(
                        new
                        {
                            ParcelaId = new Guid("5d99fd91-fcf4-4975-891b-a7ae3053ff52"),
                            BrojListeNepokretnosti = "8",
                            BrojParcele = "178",
                            KatastarskaOpstinaId = new Guid("9ef61f9a-a078-423c-96f3-c83bd2d1806f"),
                            OblikSvojineId = new Guid("f5133187-104c-4849-9d53-995e17e51094"),
                            OdvodnjavanjeId = new Guid("b2f93ba3-ec00-41af-824e-fcd1f0c60c5c"),
                            PovrsinaParcele = "250",
                            ZasticenaZonaId = new Guid("7663ebd7-8ad6-4e1a-9ed0-f0b3c07caa30")
                        },
                        new
                        {
                            ParcelaId = new Guid("985eacda-be53-4506-a7b2-339fc6acf3d6"),
                            BrojListeNepokretnosti = "7",
                            BrojParcele = "289",
                            KatastarskaOpstinaId = new Guid("9ef61f9a-a078-423c-96f3-c83bd2d1806f"),
                            OblikSvojineId = new Guid("f5133187-104c-4849-9d53-995e17e51094"),
                            OdvodnjavanjeId = new Guid("b2f93ba3-ec00-41af-824e-fcd1f0c60c5c"),
                            PovrsinaParcele = "100",
                            ZasticenaZonaId = new Guid("7663ebd7-8ad6-4e1a-9ed0-f0b3c07caa30")
                        },
                        new
                        {
                            ParcelaId = new Guid("73e47b70-c8fb-43e3-beb9-5f1b627a59bf"),
                            BrojListeNepokretnosti = "6",
                            BrojParcele = "158",
                            KatastarskaOpstinaId = new Guid("9ef61f9a-a078-423c-96f3-c83bd2d1806f"),
                            OblikSvojineId = new Guid("f5133187-104c-4849-9d53-995e17e51094"),
                            OdvodnjavanjeId = new Guid("b2f93ba3-ec00-41af-824e-fcd1f0c60c5c"),
                            PovrsinaParcele = "150",
                            ZasticenaZonaId = new Guid("875fc830-5370-4d21-82bc-ceb8ce77e5d2")
                        });
                });

            modelBuilder.Entity("ParcelaService.Entities.ZasticenaZona", b =>
                {
                    b.Property<Guid>("ZasticenaZonaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BrojZasticeneZone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ZasticenaZonaId");

                    b.ToTable("ZasticenaZona");

                    b.HasData(
                        new
                        {
                            ZasticenaZonaId = new Guid("7663ebd7-8ad6-4e1a-9ed0-f0b3c07caa30"),
                            BrojZasticeneZone = "1"
                        },
                        new
                        {
                            ZasticenaZonaId = new Guid("875fc830-5370-4d21-82bc-ceb8ce77e5d2"),
                            BrojZasticeneZone = "2"
                        },
                        new
                        {
                            ZasticenaZonaId = new Guid("0bb57a3e-5ab2-4ab0-b33b-0e3c53d713d1"),
                            BrojZasticeneZone = "3"
                        });
                });

            modelBuilder.Entity("ParcelaService.Entities.DeoParcele", b =>
                {
                    b.HasOne("ParcelaService.Entities.Klasa", "Klasa")
                        .WithMany()
                        .HasForeignKey("KlasaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ParcelaService.Entities.Kultura", "Kultura")
                        .WithMany()
                        .HasForeignKey("KulturaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ParcelaService.Entities.Obradivost", "Obradivost")
                        .WithMany()
                        .HasForeignKey("ObradivostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ParcelaService.Entities.Parcela", "Parcela")
                        .WithMany("DeloviParcele")
                        .HasForeignKey("ParcelaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Klasa");

                    b.Navigation("Kultura");

                    b.Navigation("Obradivost");

                    b.Navigation("Parcela");
                });

            modelBuilder.Entity("ParcelaService.Entities.Parcela", b =>
                {
                    b.HasOne("ParcelaService.Entities.KatastarskaOpstina", "KatastarskaOpstina")
                        .WithMany()
                        .HasForeignKey("KatastarskaOpstinaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ParcelaService.Entities.OblikSvojine", "OblikSvojine")
                        .WithMany()
                        .HasForeignKey("OblikSvojineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ParcelaService.Entities.Odvodnjavanje", "Odvodnjavanje")
                        .WithMany()
                        .HasForeignKey("OdvodnjavanjeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ParcelaService.Entities.ZasticenaZona", "ZasticenaZona")
                        .WithMany()
                        .HasForeignKey("ZasticenaZonaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KatastarskaOpstina");

                    b.Navigation("OblikSvojine");

                    b.Navigation("Odvodnjavanje");

                    b.Navigation("ZasticenaZona");
                });

            modelBuilder.Entity("ParcelaService.Entities.Parcela", b =>
                {
                    b.Navigation("DeloviParcele");
                });
#pragma warning restore 612, 618
        }
    }
}
