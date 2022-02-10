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
                Tip = "Javna licitacija"
            };
        }

        private static KupacDto GetKupac()
        {
            return new KupacDto
            {
                KupacId = Guid.Parse("febd1c29-90e7-40c2-97f3-1e88495fe98d"),
                Naziv = "Filip Ivanic",
                BrojTelefona = "069453432543",
                Email = "filip@gmailcom",
                BrojRacuna = "908 ‑ 10501 ‑ 97"
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