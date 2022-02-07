using System;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Consumer.Data;
using RabbitMQ.Consumer.Models;

namespace RabbitMQ.Consumer
{
    public static class Program
    {
        static void Main(string[] args)
        {
            string url = ConfigurationManager.AppSettings.Get("RabbitMQ");
            var factory = new ConnectionFactory { Uri = new Uri(url) };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "mailer-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine(" [*] Cekanje na podatke o mejlovima.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Primljeni podaci za slanje mejla: {0}", message);

                MailInfo mailInfo = JsonConvert.DeserializeObject<MailInfo>(message);
                Emailer.Send(mailInfo);
            };
            channel.BasicConsume(queue: "mailer-queue", autoAck: true, consumer: consumer);

            Console.WriteLine("Pritisnite [Enter] za izlaz iz programa.");
            Console.ReadLine();
        }
    }
}
