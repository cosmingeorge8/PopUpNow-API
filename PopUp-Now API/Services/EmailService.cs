using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PopUp_Now_API.Services
{
    public class EmailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /**
         * Send an email using the SendGRIP API
         */
        public async Task SendEmailAsync(Email email)
        {
            var apiKey = _configuration["SENDGRIP_API_KEY"];
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