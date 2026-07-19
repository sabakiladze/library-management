using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implimentations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(EmailSettings settings)
        {
           _settings = settings;
        }

        public void SendVerificationCode(string email, string code)
        {
            using SmtpClient smtp = new SmtpClient(_settings.SmtpServer)
            {
                Port = _settings.Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(
                 _settings.Email,
                 _settings.AppPassword
             )
            };

            MailMessage message=new MailMessage();
            message.From = new MailAddress("library.management105@gmail.com","ebei rdyr uiyr ekgl"); 
            message.To.Add(email);
            message.Subject = "Email Verification";
            message.Body = $"Your verification code is: {code}";
            smtp.Send(message);

        }
    }
}
