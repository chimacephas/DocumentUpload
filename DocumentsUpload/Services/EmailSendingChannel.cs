using DocumentsUpload.Entities;
using DocumentsUpload.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DocumentsUpload.Services
{
    public class EmailSendingChannel
    {
        private const int MaxMessagesInChannel = 100;

        private readonly Channel<EmailData> _channel;

        public EmailSendingChannel()
        {
            var options = new BoundedChannelOptions(MaxMessagesInChannel)
            {
                SingleWriter = false,
                SingleReader = true
            };

            _channel = Channel.CreateBounded<EmailData>(options);

        }

        public async Task<bool> AddMessageAsync(EmailData fileName, CancellationToken ct = default)
        {
            while (await _channel.Writer.WaitToWriteAsync(ct) && !ct.IsCancellationRequested)
            {
                if (_channel.Writer.TryWrite(fileName))
                {
                    return true;
                }
            }
            return false;
        }

        public IAsyncEnumerable<EmailData> ReadAllAsync(CancellationToken ct = default) =>
          _channel.Reader.ReadAllAsync(ct);

        public bool TryCompleteWriter(Exception ex = null) => _channel.Writer.TryComplete(ex);
    }

    public class EmailData
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public List<DocumentDto> Documents { get; set; } = new List<DocumentDto>();
    }
}
