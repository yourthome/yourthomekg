//using MailKit.Net.Smtp;
using System.Net.Mail;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Yourthome.Helpers;

namespace Yourthome.Services
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html, string from = null);
    }

    public class EmailService : IEmailService
    {

        public EmailService()
        {}

        public async void Send(string to, string subject, string html, string from = null)
        {           
            using (MailMessage email = new MailMessage())
            {
                email.Subject = subject;
                email.Body = html;
                email.IsBodyHtml = true;
                email.To.Add(to);
                email.From = new MailAddress("info@aspnet-core-signup-verification-api.com");
                // send email
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential("yourthomekg@gmail.com", "yourthome040620");
                    smtpClient.EnableSsl = true;
                    await smtpClient.SendMailAsync(email);
                }
                using (var smtpClient = new SmtpClient("smtp.mail.ru", 465))
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential("yourthomekg@mail.ru", "123abbas");
                    smtpClient.EnableSsl = true;
                    await smtpClient.SendMailAsync(email);
                }
            }
                            
        }
    }
}