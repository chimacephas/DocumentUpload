
using DocumentsUpload.Entities;
using DocumentsUpload.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsUpload.Messaging
{
    public class EmailSender : ICustomEmailSender
    {

        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public Task SendEmailAsync(string email, string subject, string message, List<DocumentDto> documents)
        {
            return Execute(email, subject, message, documents);
        }

        public Task Execute(string email, string subject, string message, List<DocumentDto> documents)
        {
            var client = new SendGridClient(_config["SendGrid:SendGridKey"]);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("chimaokoli2@gmail.com", "DocumentsUpload"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(email));

            foreach (var item in documents)
            {
                msg.AddAttachment(item.Name, Convert.ToBase64String(System.IO.File.ReadAllBytes(item.Path)));
            }

            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
