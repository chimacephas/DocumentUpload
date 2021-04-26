using DocumentsUpload.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsUpload.Repository.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserWithDocuments(string emailorTransactionId);
    }
}
