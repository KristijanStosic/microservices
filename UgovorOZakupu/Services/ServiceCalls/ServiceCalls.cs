﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UgovorOZakupu.Models.LogModel;
using UgovorOZakupu.Models.UgovorOZakupu;

namespace UgovorOZakupu.Services.ServiceCalls
{
    public class ServiceCalls : IServiceCalls
    {
        private readonly IMapper _mapper;
        private readonly IServices _services;

        public ServiceCalls(IServices services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        public async Task Log(LogLevel level, string method, string message, Exception exception = null)
        {
            var log = new LogModel
            {
                Servis = "Ugovor o zakupu API",
                Level = level,
                Metoda = method,
                Poruka = message,
                Greska = exception
            };

            await _services.Logger.SendPostRequest(log);
        }

        public async Task<UgovorOZakupuDto> GetUgovorOZakupuInfo(string token, Entities.UgovorOZakupu ugovor)
        {
            var ugovorDto = _mapper.Map<UgovorOZakupuDto>(ugovor);

            ugovorDto.Odluka = await _services.Dokument.SendGetRequest(token, ugovor.DokumentId.ToString());
            ugovorDto.JavnoNadmetanje =
                await _services.JavnoNadmetanje.SendGetRequest(token, ugovor.JavnoNadmetanjeId.ToString());
            ugovorDto.Lice = await _services.Kupac.SendGetRequest(token, ugovor.KupacId.ToString());
            ugovorDto.Ministar = await _services.Licnost.SendGetRequest(token, ugovor.LicnostId.ToString());

            return ugovorDto;
        }
    }
}