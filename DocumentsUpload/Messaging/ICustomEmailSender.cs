
using DocumentsUpload.Entities;
using DocumentsUpload.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentsUpload.Messaging
{
    public interface ICustomEmailSender
    {
        Task SendEmailAsync(string email, string subject,string message, List<DocumentDto> documents);
    }
}
