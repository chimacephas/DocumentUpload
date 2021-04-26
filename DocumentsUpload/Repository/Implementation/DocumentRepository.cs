using DocumentsUpload.Data;
using DocumentsUpload.Entities;
using DocumentsUpload.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsUpload.Repository.Implementation
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(ApplicationContext context) : base(context)
        {

        }
    }
}

