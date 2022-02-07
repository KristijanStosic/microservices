using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UgovorOZakupu.Models.Dokument;
using UgovorOZakupu.Models.JavnoNadmetanje;
using UgovorOZakupu.Models.Kupac;
using UgovorOZakupu.Models.Licnost;
using UgovorOZakupu.Models.LogModel;
using UgovorOZakupu.Models.UgovorOZakupu;

namespace UgovorOZakupu.Services.ServiceCalls
{
    public class ServiceCallsMock : IServiceCalls
    {
        private readonly IMapper _mapper;

        public ServiceCallsMock(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task Log(LogLevel level, string method, string message, Exception exception = null)
        {
            var log = new LogModel
            {
                Servis = "Ugovor o zakupu API",
                Level = level,
                Metoda = method,
                Poruka = message,
                Greska = exception
            };

            Debug.WriteLine(JsonConvert.SerializeObject(log));

            return Task.CompletedTask;
        }

        public Task<UgovorOZakupuDto> GetUgovorOZakupuInfo(string token, Entities.UgovorOZakupu ugovor)
        {
            var ugovorDto = _mapper.Map<UgovorOZakupuDto>(ugovor);

            ugovorDto.Odluka = GetDokument();
            ugovorDto.JavnoNadmetanje = GetJavnoNadmetanje();
            ugovorDto.Lice = GetKupac();
            ugovorDto.Ministar = GetLicnost();

            return Task.FromResult(ugovorDto);
        }

        private static DokumentDto GetDokument()
        {
            return new DokumentDto
            {
                ZavodniBroj = "IJEN-1/2022",
                Datum = DateTime.Now,
                DatumDonosenjaDokumenta = DateTime.Now.AddDays(3),
                TipDokumenta = "Izvod iz javne evidencije o nepokretnosti"
            };
        }

        private static JavnoNadmetanjeDto GetJavnoNadmetanje()
        {
            return new JavnoNadmetanjeDto
            {
                JavnoNadmetanjeId = Guid.NewGuid(),
                PocetnaCenaHektar = 350.50000000,
                VisinaDopuneDepozita = 50,
                PeriodZakupa = 2,
                IzlicitiranaCena = 400,
                BrojUcesnika = 5,
                Krug = 2,
                Izuzeto = false,
                Status = "Prvi krug",
                Tip = "Javna licitacija",
                Etape = new List<EtapaDto>
                {
                    new()
                    {
                        EtapaId = Guid.NewGuid(),
                        Datum = DateTime.Now.AddDays(5),
                        DanPoRedu = 1,
                        VremePocetka = DateTime.Now.ToString("HH:mm"),
                        VremeKraja = DateTime.Now.AddHours(2)
                            .ToString("HH:mm"),
                        ZavrsenaUspesno = false
                    },
                    new()
                    {
                        EtapaId = Guid.NewGuid(),
                        Datum = DateTime.Now.AddDays(2),
                        DanPoRedu = 2,
                        VremePocetka = DateTime.Now.ToString("HH:mm"),
                        VremeKraja = DateTime.Now.AddHours(4)
                            .ToString("HH:mm"),
                        ZavrsenaUspesno = true
                    }
                },
                Adresa = "Bulevar Oslobodjenja 50, 21000 Novi Sad, Srbija",
                Kupac = new JavnoNadmetanjeKupacDto
                {
                    Kupac = "Milana Milanovic",
                    Email = "milana@gmail.com",
                    BrojRacuna = "23212345",
                    BrojTelefona1 = "065145125"
                },
                OvlascenaLica = new List<OvlascenoLiceDto>
                {
                    new()
                    {
                        OvlascenoLiceId = Guid.Parse("5ED44CAB-255D-4BB7-9CC9-828EC90BFAF5"),
                        OvlascenoLice = "Petar Petrović",
                        BrojDokumenta = "0224989800025",
                        Stanovanje = "Bulevar Oslobodjenja 50, Novi Sad, Srbija",
                        BrojeviTabli = new List<BrojTableDto>()
                    },
                    new()
                    {
                        OvlascenoLiceId = Guid.Parse("5E1BFFFC-1AEE-4662-BC04-341C35B9EBDC"),
                        OvlascenoLice = "Milica Radivojević",
                        BrojDokumenta = "0263989802415",
                        Stanovanje = "Bulevar Cara Lazara 11, Novi Sad, Srbija",
                        BrojeviTabli = new List<BrojTableDto>
                        {
                            new()
                            {
                                BrojTableId = Guid.NewGuid(),
                                OznakaTable = "Talba1",
                                RbTable = 1
                            },
                            new()
                            {
                                BrojTableId = Guid.NewGuid(),
                                OznakaTable = "Talba2",
                                RbTable = 2
                            }
                        }
                    }
                },
                Kupci = new List<JavnoNadmetanjeKupacDto>
                {
                    new()
                    {
                        Kupac = "Milana Milanovic",
                        Email = "milana@gmail.com",
                        BrojRacuna = "23212345",
                        BrojTelefona1 = "065145125"
                    },
                    new()
                    {
                        Kupac = "Marko Markovic",
                        Email = "markomarkovic@gmail.com",
                        BrojRacuna = "16142232",
                        BrojTelefona1 = "063212174"
                    }
                },
                DeloviParcele = new List<DeoParceleDto>
                {
                    new()
                    {
                        BrojParcele = "2345",
                        RbDela = 1,
                        KatastarskaOpstina = "Palic",
                        Klasa = "I",
                        Kultura = "Njive",
                        Obradivost = "Obradivo",
                        Odvodnjavanje = "I",
                        PovrsinaDela = 25,
                        ZasticenaZona = "2"
                    },
                    new()
                    {
                        BrojParcele = "2345",
                        RbDela = 2,
                        KatastarskaOpstina = "Palic",
                        Klasa = "I",
                        Kultura = "Njive",
                        Obradivost = "Obradivo",
                        Odvodnjavanje = "I",
                        PovrsinaDela = 40,
                        ZasticenaZona = "2"
                    }
                }
            };
        }

        private static KupacDto GetKupac()
        {
            return new KupacDto
            {
                KupacId = Guid.Parse("febd1c29-90e7-40c2-97f3-1e88495fe98d"),
                Naziv = "Filip Ivanic",
                Faks = null,
                OstvarenaPovrsina = 500,
                ImaZabranu = false,
                DatumPocetkaZabrane = default,
                DuzinaTrajanjaZabraneGod = 0,
                BrojTelefona = "069453432543",
                BrojTelefona2 = null,
                Email = "filip@gmailcom",
                BrojRacuna = "908 ‑ 10501 ‑ 97",
                KontaktOsoba = new KontaktOsobaDto
                {
                    KontaktOsobaId = Guid.Parse("244fb7c4-aab8-4ec4-8960-e48e017bad37"),
                    Ime = "Milan",
                    Prezime = "Drazic",
                    Telefon = "0693432534",
                    Funkcija = "Funkcija1"
                },
                Prioriteti = new List<PrioritetDto>
                {
                    new()
                    {
                        PrioritetId = Guid.Parse("2578e81b-3f01-479a-b790-f52106f639f7"),
                        Opis = "Vlasnik sistema za navodnjavanje"
                    }
                },
                Adresa = new AdresaDto
                {
                    AdresaId = Guid.Parse("1c989ee3-13b2-4d3b-abeb-c4e6343eace7"),
                    Ulica = "Branka Ilica",
                    Broj = "1",
                    Mesto = "Novi Sad",
                    PostanskiBroj = "21000",
                    Drzava = "Srbija"
                },
                OvlascenaLica = new List<OvlascenoLiceDto>
                {
                    new()
                    {
                        OvlascenoLiceId = Guid.Parse("5ED44CAB-255D-4BB7-9CC9-828EC90BFAF5"),
                        OvlascenoLice = "Petar Petrović",
                        BrojDokumenta = "0224989800025",
                        Stanovanje = "Bulevar Oslobodjenja 50 Novi Sad, Srbija"
                    }
                },
                Uplate = new List<UplataDto>
                {
                    new()
                    {
                        BrojRacuna = "100-4777487000005-66",
                        PozivNaBroj = "90-555258-552-559",
                        Iznos = 1999.99,
                        SvrhaUplate = "Uplata na racun",
                        Datum = DateTime.Parse("2020-05-25")
                    },
                    new()
                    {
                        BrojRacuna = "150-2541485965214-99",
                        PozivNaBroj = "90-555258-552-559",
                        Iznos = 2052.47,
                        SvrhaUplate = "Uplata na racun",
                        Datum = DateTime.Parse("2020-05-25")
                    }
                }
            };
        }

        private static LicnostDto GetLicnost()
        {
            return new LicnostDto
            {
                LicnostId = Guid.NewGuid(),
                Ime = "Marko",
                Prezime = "Markovic",
                Funkcija = "Licitant"
            };
        }
    }
}