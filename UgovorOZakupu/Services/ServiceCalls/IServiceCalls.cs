using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UgovorOZakupu.Models.UgovorOZakupu;

namespace UgovorOZakupu.Services.ServiceCalls
{
    public interface IServiceCalls
    {
        Task Log(LogLevel level, string method, string message, Exception exception = null);
        Task<UgovorOZakupuDto> GetUgovorOZakupuInfo(string token, Entities.UgovorOZakupu ugovor);
    }
}