using DocumentsUpload.Messaging;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentsUpload.Services
{
    public class EmailSendingService : BackgroundService
    {
        private readonly EmailSendingChannel _emailSendingChannel;
        private readonly ICustomEmailSender _customEmailSender;

        public EmailSendingService(EmailSendingChannel  emailSendingChannel, ICustomEmailSender customEmailSender)
        {
            _emailSendingChannel = emailSendingChannel;
            _customEmailSender = customEmailSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var data in _emailSendingChannel.ReadAllAsync())
            {
                try
                {
                    await _customEmailSender.SendEmailAsync(data.Email, data.Subject, data.Message, data.Documents);

                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
