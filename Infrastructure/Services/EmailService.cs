using Application.Interfaces.Services;
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
            using SmtpClient smtp = new(_settings.SmtpServer)
            {
                Port = _settings.Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(
                 _settings.Email,
                 _settings.AppPassword
             )
            };

            MailMessage message = new()
            {
                From = new MailAddress(_settings.Email, "Library Management System")
            };
            message.To.Add(email);
            message.Subject = "Email Verification";
            message.Body = $"Your verification code is: {code}";
            smtp.Send(message);

        }
    }
}
