using RabbitMQ.Consumer.Models;
using System.Net.Mail;
using System.Configuration;
using System;

namespace RabbitMQ.Consumer.Data
{
    public static class Emailer
    {
        public static void Send(MailInfo mailInfo)
        {
            var mail = new MailMessage();
            var SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(ConfigurationManager.AppSettings.Get("Email"));
            mail.To.Add("majkickac99@gmail.com"); //Test email data - za prave mejlove promeniti na mailInfo.Email
            mail.Subject = "Informacije o datumu održavanja javnog nadmetanja";
            mail.IsBodyHtml = true;

            string body = System.IO.File.ReadAllText(ConfigurationManager.AppSettings.Get("Template"));
            body = body.Replace("#Kupac#", mailInfo.Kupac);
            body = body.Replace("#DatumOdrzavanja#", mailInfo.DatumOdrzavanja.ToShortDateString());
            body = body.Replace("#Adresa#", mailInfo.Adresa);
            body = body.Replace("#VremePocetka#", mailInfo.VremePocetka);
            body = body.Replace("#VremeKraja#", mailInfo.VremeKraja);
            mail.Body = body;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings.Get("Email"), ConfigurationManager.AppSettings.Get("Password"));
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}
