using JavnoNadmetanjeService.Models.Etapa;
using JavnoNadmetanjeService.Models.Other;
using JavnoNadmetanjeService.ServiceCalls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Helpers
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly ILoggerService _loggerService;
        private readonly IConfiguration _configuration; 

        public RabbitMQProducer(ILoggerService loggerService, IConfiguration configuration)
        {
            _loggerService = loggerService;
            _configuration = configuration;
        }

        public async Task SendEmailToQueue(MailInfo mailInfo)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(_configuration["Services:RabbitMQ"])
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare("mailer-queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mailInfo));
                channel.BasicPublish(exchange: "", routingKey: "mailer-queue", basicProperties: null, body: body);

                await _loggerService.Log(LogLevel.Information, "SendEmail", $"Mejl je poslat na queue. Poslati podaci: {JsonConvert.SerializeObject(mailInfo)}");
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "SendEmail", $"Greška prilikom slanja mejla. Poslati podaci: {JsonConvert.SerializeObject(mailInfo)}", ex);
            }
        }
    }
}
