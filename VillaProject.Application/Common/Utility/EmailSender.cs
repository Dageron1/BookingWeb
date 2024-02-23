using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VillaProject.Application.Common.Utility
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //logic to send email

            //var client = new SendGridClient(SendGridSecret);


            //var to = new EmailAddress(email);
            //var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            MailMessage message = new MailMessage();
            message.To.Add(email);
            message.Subject = subject;
            message.From = new MailAddress("support@dagerondev.com");
            message.Body = $"<html><body> {htmlMessage}</body></html>";
            message.IsBodyHtml = true;


            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("stryhaliouyauheni@gmail.com", "ejjqyssmdgbtqrxq");
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(message);
            }

        }
    }
}
