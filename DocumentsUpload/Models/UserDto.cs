using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsUpload.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TransactionNumber { get; set; }
        public List<DocumentDto> Documents { get; set; } = new List<DocumentDto>();
    }
}
