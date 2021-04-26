using System.ComponentModel.DataAnnotations;

namespace DocumentsUpload.Models
{
    public class UserDocuments
    {
        [Required]
        public string EmailOrTransactionId { get; set; }
    }
}
