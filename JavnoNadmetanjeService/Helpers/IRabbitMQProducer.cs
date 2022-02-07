using JavnoNadmetanjeService.Models.Other;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Helpers
{
    public interface IRabbitMQProducer
    {
        Task SendEmailToQueue(MailInfo mailInfo); 
    }
}
