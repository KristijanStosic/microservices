﻿using KupacService.Entities;
using KupacService.Entities.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Data.Interfaces
{
    public interface IFizickoLiceRepository
    {

        Task<List<FizickoLice>> GetFizickoLice(string ime = null,string prezime = null,string brojRacuna = null);
        Task<FizickoLice> GetFizickoLiceById(Guid kupacId);
        Task<FizickoLice> CreateFizickoLice(FizickoLice fizickoLice);
        Task DeleteFizickoLice(Guid kupacId);
        Task SaveChangesAsync();
        Task UpdateKupacOvlascenoLice(FizickoLice fizickoLice);
        Task<List<KupacOvlascenoLice>> GetKupacOvlascenoLiceByOvlascenoLiceId(Guid ovlascenoLiceId);
    }
}
