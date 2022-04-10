using System;
using System.Threading.Tasks;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PopUp_Now_API.Services
{
    public class EmailService : IMailService
    {
        public async Task SendEmailAsync(Email email)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRIP_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("cosminmcl@gmail.com", "Pop-up Store API");
            var subject = email.Subject;
            var to = new EmailAddress(email.ToEmail);
            var plainTextContent = email.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, plainTextContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}