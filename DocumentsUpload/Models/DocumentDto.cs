using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsUpload.Models
{
    public class DocumentDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public Guid UserId { get; set; }
    }
}
