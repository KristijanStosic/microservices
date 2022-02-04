using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using UgovorOZakupu.Models.Dokument;
using UgovorOZakupu.Models.JavnoNadmetanje;
using UgovorOZakupu.Models.UgovorOZakupu;

namespace UgovorOZakupu.Services.ServiceCalls
{
    public class ServiceCallsMock : IServiceCalls
    {
        private IMapper _mapper;

        public ServiceCallsMock(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<UgovorOZakupuDto> GetUgovorOZakupuInfo(Entities.UgovorOZakupu ugovor)
        {
            var ugovorDto = _mapper.Map<UgovorOZakupuDto>(ugovor);
            
            ugovorDto.Dokument = GetDokument();
            ugovorDto.JavnoNadmetanje = GetJavnoNadmetanje();
            
            return Task.FromResult(ugovorDto);
        }

        private DokumentDto GetDokument()
        {
            return new DokumentDto
            {
                ZavodniBroj = "IJEN-1/2022",
                Datum = DateTime.Now,
                DatumDonosenjaDokumenta = DateTime.Now.AddDays(3),
                TipDokumenta = "Izvod iz javne evidencije o nepokretnosti"
            };
        }

        private JavnoNadmetanjeDto GetJavnoNadmetanje()
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
                        ZavrsenaUspesno = true,
                    }
                },
                Adresa = "Bulevar Oslobodjenja 50, 21000 Novi Sad, Srbija",
                Kupac = new KupacDto
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
                        OvlascenoLice = "Petar Petrović",
                        BrojDokumenta = "0224989800025",
                        Stanovanje = "Bulevar Oslobodjenja 50, Novi Sad, Srbija"
                    },
                    new()
                    {
                        OvlascenoLice = "Milica Radivojević",
                        BrojDokumenta = "0263989802415",
                        Stanovanje = "Bulevar Cara Lazara 11, Novi Sad, Srbija"
                    }
                },
                Kupci = new List<KupacDto>
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
    }
}