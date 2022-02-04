using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoggerService.Data;
using LoggerService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoggerService.Controllers
{
    [Route("api/logger")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        public LoggerController(ILoggerManager logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Kreira se poruka koja se upisuje nakon  konkatenacije servisa sa kojeg dolazi poruka, metoda koja je upisana i poruka o metodi
        /// </summary>
        /// <param name="logModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostLogMessage([FromBody] LogModel logModel)
        {
            try
            {
                string poruka = logModel.Servis + " | " + logModel.Metoda + " | " + logModel.Poruka;

                if(logModel.Level == LogLevel.Information)
                {
                    _logger.LogInfo(poruka);
                }

                else if(logModel.Level == LogLevel.Error)
                {
                    _logger.LogError(logModel.Greska, poruka);
                }

                else if (logModel.Level == LogLevel.Debug)
                {
                    _logger.LogDebug(poruka);
                }

                else if (logModel.Level == LogLevel.Warning)
                {
                    _logger.LogWarn(poruka);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , "Desila se greska prilikom upisa u log fajl.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom upisa u log fajl");
            }
        }

    }
}
