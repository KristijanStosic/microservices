﻿using JavnoNadmetanjeService.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Data
{
    public interface IStatusRepository
    {
        Task<List<Status>> GetAllStatus(string nazivStatusa = null);
        Task<Status> GetStatusById(Guid statusId);
        Task<Status> CreateStatus(Status status);
        Task DeleteStatus(Guid statusId);
        Task UpdateStatus();
    }
}
